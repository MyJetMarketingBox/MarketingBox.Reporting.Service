using System.Threading.Tasks;
using MarketingBox.Registration.Service.Messages.Registrations;
using MarketingBox.Reporting.Service.Domain;
using MarketingBox.Reporting.Service.Engines.Interfaces;
using MarketingBox.Reporting.Service.Repositories.Interfaces;
using MarketingBox.Sdk.Common.Enums;
using RegistrationDetails = MarketingBox.Reporting.Service.Domain.Models.Registrations.RegistrationDetails;

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
            RegistrationUid = message.GeneralInfoInternal.RegistrationUid,
            CreatedAt = message.GeneralInfoInternal.CreatedAt.ToUtc(),
            TenantId = message.TenantId,
            FirstName = message.GeneralInfoInternal.FirstName,
            LastName = message.GeneralInfoInternal.LastName,
            Email = message.GeneralInfoInternal.Email,
            Phone = message.GeneralInfoInternal.Phone,
            Ip = message.GeneralInfoInternal.Ip,
            CountryAlfa2Code = message.GeneralInfoInternal.CountryAlfa2Code,
            AffiliateId = message.RouteInfo.AffiliateId,
            AffiliateName = message.RouteInfo.AffiliateName,
            BrandId = message.RouteInfo.BrandId,
            CampaignId = message.RouteInfo.CampaignId,
            ConversionDate = message.RouteInfo.ConversionDate?.ToUtc(),
            DepositDate = message.RouteInfo.DepositDate?.ToUtc(),
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
            CustomerBrand = message.RouteInfo.BrandInfo.Brand,
            CustomerId = message.RouteInfo.BrandInfo.CustomerId,
            CustomerLoginUrl = message.RouteInfo.BrandInfo.LoginUrl,
            CustomerToken = message.RouteInfo.BrandInfo.Token,
            Integration = message.RouteInfo.Integration,
            IntegrationId = message.RouteInfo.IntegrationId ?? default,
            RegistrationId = message.GeneralInfoInternal.RegistrationId,
            Status = message.RouteInfo.Status.MapEnum<RegistrationStatus>(),
            UpdateMode = message.RouteInfo.UpdateMode
                .MapEnum<DepositUpdateMode>(),
            AutologinUsed = message.RouteInfo.AutologinUsed,
            Password = message.GeneralInfoInternal.Password
        };
    }
}