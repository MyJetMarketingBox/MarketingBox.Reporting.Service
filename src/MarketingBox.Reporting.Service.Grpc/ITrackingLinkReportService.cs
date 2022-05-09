using System.ServiceModel;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Grpc.Requests.TrackingLinks;
using MarketingBox.Sdk.Common.Models.Grpc;

namespace MarketingBox.Reporting.Service.Grpc;

[ServiceContract]
public interface ITrackingLinkReportService
{
    [OperationContract]
    Task<Response<Domain.Models.TrackingLinks.TrackingLink>> GetAsync(TrackingLinkByClickIdRequest request);
}