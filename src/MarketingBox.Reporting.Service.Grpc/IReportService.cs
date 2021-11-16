using System.ServiceModel;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Grpc.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Models.Reports.Requests;

namespace MarketingBox.Reporting.Service.Grpc
{
    [ServiceContract]
    public interface IReportService
    {
        [OperationContract]
        Task<ReportSearchResponse> SearchAsync(ReportSearchRequest request);

        [OperationContract]
        Task<ReportByDaySearchResponse> SearchByDayAsync(ReportByDaySearchRequest request);
    }
}
