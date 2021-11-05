using DotNetCoreDecorators;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using System;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Domain.Models.Brands;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.MyNoSql.Brands;
using MarketingBox.Registration.Service.Messages.Registrations;
using MarketingBox.Reporting.Service.Domain.Extensions;
using MarketingBox.Reporting.Service.Domain.Lead;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Deposits;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Reports;
using Microsoft.EntityFrameworkCore;

namespace MarketingBox.Reporting.Service.Subscribers
{
    public class RegistrationUpdateMessageSubscriber
    {
        private readonly ILogger<RegistrationUpdateMessageSubscriber> _logger;
        private readonly IMyNoSqlServerDataReader<BrandNoSql> _campDataReader;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
        private readonly IBrandService _brandService;

        public RegistrationUpdateMessageSubscriber(
            ISubscriber<MarketingBox.Registration.Service.Messages.Registrations.RegistrationUpdateMessage> subscriber,
            ILogger<RegistrationUpdateMessageSubscriber> logger,
            IMyNoSqlServerDataReader<BrandNoSql> campDataReader,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder,
            IBrandService brandService)
        {
            _logger = logger;
            _campDataReader = campDataReader;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            _brandService = brandService;
            subscriber.Subscribe(Consume);
        }

        private async ValueTask Consume(MarketingBox.Registration.Service.Messages.Registrations.RegistrationUpdateMessage message)
        {
            _logger.LogInformation("Consuming message {@context}", message);
            var brandNoSql = _campDataReader.Get(
                BrandNoSql.GeneratePartitionKey(message.TenantId),
                BrandNoSql.GenerateRowKey(message.RouteInfo.BrandId));

            decimal leadPayoutAmount;
            decimal leadRevenueAmount;
            if (brandNoSql == null)
            {
                var brandResponse = await _brandService.GetAsync(new () { BrandId = message.RouteInfo.BrandId });

                if (brandResponse?.Brand == null)
                {
                    _logger.LogWarning($"brand can not be found {message.RouteInfo.BrandId} " + "{@context}", message);
                    throw new Exception($"brand can not be found {message.RouteInfo.BrandId} ");
                }

                leadPayoutAmount = brandResponse.Brand.Payout.Plan == Plan.CPL ? brandResponse.Brand.Payout.Amount : 0;
                leadRevenueAmount = brandResponse.Brand.Revenue.Plan == Plan.CPL ? brandResponse.Brand.Revenue.Amount : 0;
            }
            else
            {
                leadPayoutAmount = brandNoSql.Payout.Plan == Plan.CPL ? brandNoSql.Payout.Amount : 0;
                leadRevenueAmount = brandNoSql.Revenue.Plan == Plan.CPL ? brandNoSql.Revenue.Amount : 0;
            }

            decimal depositPayoutAmount;
            decimal depositRevenueAmount;

            if (brandNoSql != null)
            {
                depositPayoutAmount = brandNoSql.Payout.Plan == Plan.CPA ? brandNoSql.Payout.Amount : 0;
                depositRevenueAmount = brandNoSql.Revenue.Plan == Plan.CPA ? brandNoSql.Revenue.Amount : 0;
            }
            else
            {
                var brandResponse = await _brandService.GetAsync(new () { BrandId = message.RouteInfo.CampaignId });

                //Error
                if (brandResponse?.Brand == null)
                {
                    _logger.LogError("There is no brand! Skipping message: {@context}", message);

                    throw new Exception("Retry!");
                }

                if (brandResponse.Error != null)
                {
                    _logger.LogError("Error from affiliate service while processing message: {@context}", message);

                    throw new Exception("Retry!");
                }

                if (brandResponse.Brand == null)
                {
                    _logger.LogError("There is no brand! Skipping message: {@context}", message);
                    return;
                }

                depositPayoutAmount = brandResponse.Brand.Payout.Plan == Plan.CPA ? brandResponse.Brand.Payout.Amount : 0;
                depositRevenueAmount = brandResponse.Brand.Revenue.Plan == Plan.CPA ? brandResponse.Brand.Revenue.Amount : 0;
            }

            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

            var registration = MapToReadModel(message);
            await using var transaction = context.Database.BeginTransaction();
            var isDeposit = registration.Status == LeadStatus.Deposited ||
                            registration.Status == LeadStatus.Approved;
            ReportEntity reportEntity;

            try
            {
                var affectedRowsCount = await context.Registrations.Upsert(registration)
                    .UpdateIf((prevLead) => prevLead.Sequence < registration.Sequence)
                    .RunAsync();

                if (affectedRowsCount != 1)
                {
                    _logger.LogInformation("There is nothing to update: {@context}", message);
                    await transaction.RollbackAsync();
                    return;
                }

                if (isDeposit)
                {
                    var deposit = MapDeposit(message);
                    var affectedRowsDepositCount = await context.Deposits.Upsert(deposit)
                        .UpdateIf((depositPrev) => depositPrev.Sequence < deposit.Sequence)
                        .RunAsync();

                    if (affectedRowsDepositCount != 1)
                    {
                        _logger.LogInformation("There is nothing to update: {@context}", message);
                        await transaction.RollbackAsync();
                        return;
                    }

                    reportEntity = new ReportEntity()
                    {
                        CreatedAt = registration.CreatedAt,
                        AffiliateId = registration.AffiliateId,
                        CampaignId = registration.CampaignId,
                        IntegrationId = registration.IntegrationId,
                        BrandId = registration.BrandId,
                        RegistrationId = registration.RegistrationId,
                        Payout = depositPayoutAmount,
                        ReportType = ReportType.Deposit,
                        Revenue = depositRevenueAmount,
                        TenantId = registration.TenantId,
                        UniqueId = registration.UniqueId,
                    };
                }
                else
                {
                    reportEntity = new ReportEntity()
                    {
                        CreatedAt = registration.CreatedAt,
                        AffiliateId = registration.AffiliateId,
                        CampaignId = registration.CampaignId,
                        IntegrationId = registration.IntegrationId,
                        BrandId = registration.BrandId,
                        RegistrationId = registration.RegistrationId,
                        Payout = leadPayoutAmount,
                        ReportType = ReportType.Registration,
                        Revenue = leadRevenueAmount,
                        TenantId = registration.TenantId,
                        UniqueId = registration.UniqueId
                    };
                }

                await context.Reports.Upsert(reportEntity).RunAsync();

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "Error during consumptions {@context}", message);
                await transaction.RollbackAsync();

                throw;
            }

            _logger.LogInformation("Has been consumed {@context}", message);
        }

        private static Deposit MapDeposit(RegistrationUpdateMessage message)
        {
            return new Deposit()
            {
                RegistrationId = message.GeneralInfo.RegistrationId,
                Sequence = message.Sequence,
                AffiliateId = message.RouteInfo.AffiliateId,
                CampaignId = message.RouteInfo.CampaignId,
                IntegrationId = message.RouteInfo.IntegrationId,
                BrandId = message.RouteInfo.BrandId,
                ConversionDate = message.RouteInfo.ConversionDate,
                Country = message.GeneralInfo.Country,
                CustomerId = message.RouteInfo.CustomerInfo.CustomerId,
                Email = message.GeneralInfo.Email,
                RegisterDate = message.GeneralInfo.CreatedAt.ToUtc(),
                CreatedAt = message.GeneralInfo.CreatedAt.ToUtc(),
                TenantId = message.TenantId, 
                Type = message.RouteInfo.ApprovedType.MapEnum<MarketingBox.Reporting.Service.Domain.Deposit.ApprovedType>(),
                UniqueId = message.GeneralInfo.UniqueId,
                BrandStatus = message.RouteInfo.CrmCrmStatus,
            };
        }

        private static Postgres.ReadModels.Leads.Registration MapToReadModel(RegistrationUpdateMessage message)
        {
            return new Postgres.ReadModels.Leads.Registration()
            {
                So = message.AdditionalInfo.So,
                Sub = message.AdditionalInfo.Sub,
                Sub1 = message.AdditionalInfo.Sub1,
                Sub10 = message.AdditionalInfo.Sub10,
                Sub2 = message.AdditionalInfo.Sub2,
                Sub3 = message.AdditionalInfo.Sub3,
                Sub4 = message.AdditionalInfo.Sub4,
                Sub5 = message.AdditionalInfo.Sub5,
                Sub6 = message.AdditionalInfo.Sub6,
                Sub7 = message.AdditionalInfo.Sub7,
                Sub8 = message.AdditionalInfo.Sub8,
                Sub9 = message.AdditionalInfo.Sub9,

                AffiliateId = message.RouteInfo.AffiliateId,
                CampaignId = message.RouteInfo.CampaignId,
                IntegrationId = message.RouteInfo.IntegrationId,
                BrandId = message.RouteInfo.BrandId,
                CreatedAt = DateTime.SpecifyKind(message.GeneralInfo.CreatedAt, DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(message.GeneralInfo.UpdatedAt, DateTimeKind.Utc),
                Email = message.GeneralInfo.Email,
                ConversionDate = message.RouteInfo.ConversionDate != null ?DateTime.SpecifyKind(message.RouteInfo.ConversionDate.Value, DateTimeKind.Utc) : null,
                DepositDate = message.RouteInfo.DepositDate != null ? DateTime.SpecifyKind(message.RouteInfo.DepositDate.Value, DateTimeKind.Utc) : null,
                FirstName = message.GeneralInfo.FirstName,
                Ip = message.GeneralInfo.Ip,
                LastName = message.GeneralInfo.LastName,
                RegistrationId = message.GeneralInfo.RegistrationId,
                Phone = message.GeneralInfo.Phone,
                Sequence = message.Sequence,
                //CrmStatus = message.RouteInfo.CrmCrmStatus.MapEnum<MarketingBox.Reporting.Service.Domain.Registration.LeadCrmStatus>(),
                TenantId = message.TenantId,
                Status = message.RouteInfo.Status.MapEnum<MarketingBox.Reporting.Service.Domain.Lead.LeadStatus>(),
                UniqueId = message.GeneralInfo.UniqueId,
                Country = message.GeneralInfo.Country,
            };
        }
    }
}
