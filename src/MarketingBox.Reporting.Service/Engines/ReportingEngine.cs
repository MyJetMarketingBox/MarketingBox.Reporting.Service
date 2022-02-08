using System;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Domain.Models.Brands;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.MyNoSql.Brands;
using MarketingBox.Registration.Service.Messages.Registrations;
using MarketingBox.Reporting.Service.Domain;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Repositories;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using RegistrationDetails = MarketingBox.Reporting.Service.Domain.Models.RegistrationDetails;

namespace MarketingBox.Reporting.Service.Engines
{
    public class ReportingEngine : IReportingEngine
    {
        private readonly IMyNoSqlServerDataReader<BrandNoSql> _campDataReader;
        private readonly IBrandService _brandService;
        private readonly IRegistrationDetailsRepository _repository;
        private ILogger<ReportingEngine> _logger;

        public ReportingEngine(
            IMyNoSqlServerDataReader<BrandNoSql> campDataReader,
            IBrandService brandService,
            IRegistrationDetailsRepository repository,
            ILogger<ReportingEngine> logger)
        {
            _campDataReader = campDataReader;
            _brandService = brandService;
            _repository = repository;
            _logger = logger;
        }


        public async Task ProcessMessageAsync(RegistrationUpdateMessage message)
        {
            var customer = MapRegistrationDetails(message);
            await _repository.SaveAsync(MapRegistrationDetails(message));
        }

        private async Task CalculateAmounts(RegistrationUpdateMessage message)
        {
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
        }
        

        private static RegistrationDetails MapRegistrationDetails(RegistrationUpdateMessage message)
        {
            return new RegistrationDetails()
            {
                RegistrationUid = message.GeneralInfo.RegistrationUId,
                CreatedAt = message.GeneralInfo.CreatedAt.ToUtc(),
                TenantId = message.TenantId,
                FirstName = message.GeneralInfo.FirstName,
                LastName = message.GeneralInfo.LastName,
                Email = message.GeneralInfo.Email,
                Phone = message.GeneralInfo.Phone,
                Ip = message.GeneralInfo.Ip,
                Country = message.GeneralInfo.Country,
                AffiliateId = message.RouteInfo.AffiliateId,
                AffiliateName = message.RouteInfo.AffiliateName,
                BrandId = message.RouteInfo.BrandId,
                CampaignId = message.RouteInfo.CampaignId,
                ConversionDate = message.RouteInfo.ConversionDate,
                DepositDate = message.RouteInfo.DepositDate,
                CrmStatus = message.RouteInfo.CrmStatus.MapEnum<CrmStatus>(),
                AffCode = message.AdditionalInfo.AffCode,
                Funnel = message.AdditionalInfo.Funnel,
                Sub1 = message.AdditionalInfo.Sub1,
                Sub2 = message.AdditionalInfo.Sub2,
                Sub3 = message.AdditionalInfo.Sub3,
                Sub4 = message.AdditionalInfo.Sub4,
                Sub5 = message.AdditionalInfo.Sub5,
                Sub6 = message.AdditionalInfo.Sub6,
                Sub7 = message.AdditionalInfo.Sub7,
                Sub8 = message.AdditionalInfo.Sub8,
                Sub9 = message.AdditionalInfo.Sub9,
                Sub10 = message.AdditionalInfo.Sub10,
                CustomerBrand = message.RouteInfo.CustomerInfo.Brand,
                CustomerId = message.RouteInfo.CustomerInfo.CustomerId,
                CustomerLoginUrl = message.RouteInfo.CustomerInfo.LoginUrl,
                CustomerToken = message.RouteInfo.CustomerInfo.Token,
                Integration = message.RouteInfo.Integration,
                IntegrationId = message.RouteInfo.IntegrationId,
                RegistrationId = message.GeneralInfo.RegistrationId,
                Status = message.RouteInfo.Status.MapEnum<RegistrationStatus>(),
                UpdateMode = message.RouteInfo.UpdateMode
                    .MapEnum<DepositUpdateMode>(),
            };
        }
    }
}