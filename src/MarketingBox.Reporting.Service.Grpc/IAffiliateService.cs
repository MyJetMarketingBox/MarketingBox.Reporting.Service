using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Domain.Models.Registrations;
using MarketingBox.Reporting.Service.Grpc.Requests.Registrations;
using MarketingBox.Sdk.Common.Models.Grpc;

namespace MarketingBox.Reporting.Service.Grpc;

[ServiceContract]
public interface IAffiliateService
{
    [OperationContract]
    Task<Response<IReadOnlyCollection<RegistrationDetails>>> GetRegistrations(RegistrationsByAffiliateRequest request);

    [OperationContract]
    Task<Response<RegistrationDetails>> GetRegistration(RegistrationByAffiliateRequest request);
}
