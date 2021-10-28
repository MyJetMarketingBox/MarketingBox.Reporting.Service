using DotNetCoreDecorators;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Domain.Models.Campaigns;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Models.Campaigns.Requests;
using MarketingBox.Affiliate.Service.MyNoSql.Campaigns;
using MarketingBox.Registration.Service.Messages.Leads;
using MarketingBox.Reporting.Service.Domain.Extensions;
using MarketingBox.Reporting.Service.Domain.Lead;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Deposits;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Leads;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Reports;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Z.EntityFramework.Plus;

namespace MarketingBox.Reporting.Service.Subscribers
{
    public class LeadUpdateMessageSubscriber
    {
        private readonly ILogger<LeadUpdateMessageSubscriber> _logger;
        private readonly IMyNoSqlServerDataReader<CampaignNoSql> _campDataReader;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
        private readonly ICampaignService _campaignService;

        public LeadUpdateMessageSubscriber(
            ISubscriber<MarketingBox.Registration.Service.Messages.Leads.LeadUpdateMessage> subscriber,
            ILogger<LeadUpdateMessageSubscriber> logger,
            IMyNoSqlServerDataReader<CampaignNoSql> campDataReader,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder,
            ICampaignService campaignService)
        {
            _logger = logger;
            _campDataReader = campDataReader;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            _campaignService = campaignService;
            subscriber.Subscribe(Consume);
        }

        private async ValueTask Consume(MarketingBox.Registration.Service.Messages.Leads.LeadUpdateMessage message)
        {
            _logger.LogInformation("Consuming message {@context}", message);
            var campaignNoSql = _campDataReader.Get(
                CampaignNoSql.GeneratePartitionKey(message.TenantId),
                CampaignNoSql.GenerateRowKey(message.RouteInfo.CampaignId));

            decimal leadPayoutAmount;
            decimal leadRevenueAmount;
            if (campaignNoSql == null)
            {
                var campaign = await _campaignService.GetAsync(new CampaignGetRequest() { CampaignId = message.RouteInfo.CampaignId });

                if (campaign?.Campaign == null)
                {
                    _logger.LogWarning($"Company can not be found {message.RouteInfo.CampaignId} " + "{@context}", message);
                    throw new Exception($"Company can not be found {message.RouteInfo.CampaignId} ");
                }

                leadPayoutAmount = campaign.Campaign.Payout.Plan == Plan.CPL ? campaign.Campaign.Payout.Amount : 0;
                leadRevenueAmount = campaign.Campaign.Revenue.Plan == Plan.CPL ? campaign.Campaign.Revenue.Amount : 0;
            }
            else
            {
                leadPayoutAmount = campaignNoSql.Payout.Plan == Plan.CPL ? campaignNoSql.Payout.Amount : 0;
                leadRevenueAmount = campaignNoSql.Revenue.Plan == Plan.CPL ? campaignNoSql.Revenue.Amount : 0;
            }

            decimal depositPayoutAmount;
            decimal depositRevenueAmount;

            if (campaignNoSql != null)
            {
                depositPayoutAmount = campaignNoSql.Payout.Plan == Plan.CPA ? campaignNoSql.Payout.Amount : 0;
                depositRevenueAmount = campaignNoSql.Revenue.Plan == Plan.CPA ? campaignNoSql.Revenue.Amount : 0;
            }
            else
            {
                var campaign = await _campaignService.GetAsync(new CampaignGetRequest() { CampaignId = message.RouteInfo.CampaignId });

                //Error
                if (campaign?.Campaign == null)
                {
                    _logger.LogError("There is no campaign! Skipping message: {@context}", message);

                    throw new Exception("Retry!");
                }

                if (campaign.Error != null)
                {
                    _logger.LogError("Error from affiliate service while processing message: {@context}", message);

                    throw new Exception("Retry!");
                }

                if (campaign.Campaign == null)
                {
                    _logger.LogError("There is no campaign! Skipping message: {@context}", message);
                    return;
                }

                depositPayoutAmount = campaign.Campaign.Payout.Plan == Plan.CPA ? campaign.Campaign.Payout.Amount : 0;
                depositRevenueAmount = campaign.Campaign.Revenue.Plan == Plan.CPA ? campaign.Campaign.Revenue.Amount : 0;
            }

            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

            var lead = MapToReadModel(message);
            await using var transaction = context.Database.BeginTransaction();
            var isDeposit = lead.Status == LeadStatus.Deposited ||
                            lead.Status == LeadStatus.Approved;
            ReportEntity reportEntity;

            try
            {
                var affectedRowsCount = await context.Leads.Upsert(lead)
                    .UpdateIf((prevLead) => prevLead.Sequence < lead.Sequence)
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
                        CreatedAt = lead.CreatedAt,
                        AffiliateId = lead.AffiliateId,
                        BoxId = lead.BoxId,
                        BrandId = lead.BrandId,
                        CampaignId = lead.CampaignId,
                        LeadId = lead.LeadId,
                        Payout = depositPayoutAmount,
                        ReportType = ReportType.Deposit,
                        Revenue = depositRevenueAmount,
                        TenantId = lead.TenantId,
                        UniqueId = lead.UniqueId,
                    };
                }
                else
                {
                    reportEntity = new ReportEntity()
                    {
                        CreatedAt = lead.CreatedAt,
                        AffiliateId = lead.AffiliateId,
                        BoxId = lead.BoxId,
                        BrandId = lead.BrandId,
                        CampaignId = lead.CampaignId,
                        LeadId = lead.LeadId,
                        Payout = leadPayoutAmount,
                        ReportType = ReportType.Lead,
                        Revenue = leadRevenueAmount,
                        TenantId = lead.TenantId,
                        UniqueId = lead.UniqueId
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

        private static Deposit MapDeposit(LeadUpdateMessage message)
        {
            return new Deposit()
            {
                LeadId = message.GeneralInfo.LeadId,
                Sequence = message.Sequence,
                AffiliateId = message.RouteInfo.AffiliateId,
                BoxId = message.RouteInfo.BoxId,
                BrandId = message.RouteInfo.BrandId,
                CampaignId = message.RouteInfo.CampaignId,
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

        private static Postgres.ReadModels.Leads.Lead MapToReadModel(LeadUpdateMessage message)
        {
            return new Postgres.ReadModels.Leads.Lead()
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
                BoxId = message.RouteInfo.BoxId,
                BrandId = message.RouteInfo.BrandId,
                CampaignId = message.RouteInfo.CampaignId,
                CreatedAt = DateTime.SpecifyKind(message.GeneralInfo.CreatedAt, DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(message.GeneralInfo.UpdatedAt, DateTimeKind.Utc),
                Email = message.GeneralInfo.Email,
                ConversionDate = message.RouteInfo.ConversionDate != null ?DateTime.SpecifyKind(message.RouteInfo.ConversionDate.Value, DateTimeKind.Utc) : null,
                DepositDate = message.RouteInfo.DepositDate != null ? DateTime.SpecifyKind(message.RouteInfo.DepositDate.Value, DateTimeKind.Utc) : null,
                FirstName = message.GeneralInfo.FirstName,
                Ip = message.GeneralInfo.Ip,
                LastName = message.GeneralInfo.LastName,
                LeadId = message.GeneralInfo.LeadId,
                Phone = message.GeneralInfo.Phone,
                Sequence = message.Sequence,
                //CrmStatus = message.RouteInfo.CrmCrmStatus.MapEnum<MarketingBox.Reporting.Service.Domain.Lead.LeadCrmStatus>(),
                TenantId = message.TenantId,
                Status = message.RouteInfo.Status.MapEnum<MarketingBox.Reporting.Service.Domain.Lead.LeadStatus>(),
                UniqueId = message.GeneralInfo.UniqueId,
                Country = message.GeneralInfo.Country,
            };
        }
    }
}
