using System.ServiceModel;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Grpc.Models.RegistrationsByAffiliate;

namespace MarketingBox.Reporting.Service.Grpc;

[ServiceContract]
public interface IAffiliateService
{
    [OperationContract]
    Task<RegistrationsByAffiliateResponse> GetRegistrations(RegistrationsByAffiliateRequest request);

    [OperationContract]
    Task<RegistrationByAffiliateResponse> GetRegistration(RegistrationByAffiliateRequest request);
}
