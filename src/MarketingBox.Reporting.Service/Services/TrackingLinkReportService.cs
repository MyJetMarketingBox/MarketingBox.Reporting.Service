using System;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Requests.TrackingLinks;
using MarketingBox.Reporting.Service.Repositories.Interfaces;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.Grpc;

namespace MarketingBox.Reporting.Service.Services;

public class TrackingLinkReportService : ITrackingLinkReportService
{
    private readonly ITrackingLinkRepository _repository;
    public TrackingLinkReportService(ITrackingLinkRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Response<Domain.Models.TrackingLinks.TrackingLink>> GetAsync(TrackingLinkByClickIdRequest request)
    {
        try
        {
            request.ValidateEntity();
            
            var result =  await _repository.GetAsync(request.ClickId.Value);
            return new Response<Domain.Models.TrackingLinks.TrackingLink>()
            {
                Data = result,
                Status = ResponseStatus.Ok
            };
        }
        catch (Exception e)
        {
            return e.FailedResponse<Domain.Models.TrackingLinks.TrackingLink>();
        }
    }
}