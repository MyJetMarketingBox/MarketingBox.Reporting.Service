using System.Threading.Tasks;
using MarketingBox.Registration.Service.Messages.Registrations;
using MarketingBox.Reporting.Service.Domain;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Repositories;
using RegistrationDetails = MarketingBox.Reporting.Service.Domain.Models.RegistrationDetails;

namespace MarketingBox.Reporting.Service.Engines;

public class ReportingEngine : IReportingEngine
{
    private readonly IRegistrationDetailsRepository _repository;

    public ReportingEngine(IRegistrationDetailsRepository repository)
    {
        _repository = repository;
    }


    public async Task ProcessMessageAsync(RegistrationUpdateMessage message)
    {
        await _repository.SaveAsync(MapRegistrationDetails(message));
    }

    private static RegistrationDetails MapRegistrationDetails(RegistrationUpdateMessage message)
    {
        return new RegistrationDetails
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
            AutologinUsed = message.RouteInfo.AutologinUsed
        };
    }
}