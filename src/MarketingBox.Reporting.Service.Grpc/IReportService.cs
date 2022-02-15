using System.ServiceModel;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models.Reports.Requests;
using MarketingBox.Reporting.Service.Grpc.Models.Reports;

namespace MarketingBox.Reporting.Service.Grpc
{
    [ServiceContract]
    public interface IReportService
    {
        [OperationContract]
        Task<ReportSearchResponse> SearchAsync(ReportSearchRequest request);
    }
}
