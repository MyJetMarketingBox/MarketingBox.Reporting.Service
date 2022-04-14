using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Requests.Reports;
using MarketingBox.Sdk.Common.Models.Grpc;

namespace MarketingBox.Reporting.Service.Grpc
{
    [ServiceContract]
    public interface IReportService
    {
        [OperationContract]
        Task<Response<IReadOnlyCollection<Report>>> SearchAsync(ReportSearchRequest request);
    }
}
