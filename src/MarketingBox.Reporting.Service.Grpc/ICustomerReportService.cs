using System.ServiceModel;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Grpc.Models;

namespace MarketingBox.Reporting.Service.Grpc
{
    
    [ServiceContract]
    public interface ICustomerReportService
    {
        [OperationContract]
        Task<GetCustomersReportResponse> GetCustomersReport(GetCustomersReportRequest request);
        
        [OperationContract]
        Task<GetCustomerReportResponse> GetCustomerReport(GetCustomerReportRequest request);
    }
}