using System.ServiceModel;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Grpc.Models.Leads;
using MarketingBox.Reporting.Service.Grpc.Models.Leads.Requests;
using MarketingBox.Reporting.Service.Grpc.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Models.Reports.Requests;

namespace MarketingBox.Reporting.Service.Grpc
{
    [ServiceContract]
    public interface IRegistrationService
    {
        [OperationContract]
        Task<RegistrationSearchResponse> SearchAsync(RegistrationSearchRequest request);
    }
}