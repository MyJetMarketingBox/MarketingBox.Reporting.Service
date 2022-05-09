using AutoMapper;

namespace MarketingBox.Reporting.Service.MapperProfiles;


public class TrackingLinkMapperProfile:Profile
{
    public TrackingLinkMapperProfile()
    {
        CreateMap<TrackingLink.Service.Domain.Models.TrackingLink, Domain.Models.TrackingLinks.TrackingLink>();
        CreateMap<TrackingLink.Service.Domain.Models.LinkParameterNames, Domain.Models.TrackingLinks.LinkParameterNames>();
        CreateMap<TrackingLink.Service.Domain.Models.LinkParameterValues, Domain.Models.TrackingLinks.LinkParameterValues>();
    }
}