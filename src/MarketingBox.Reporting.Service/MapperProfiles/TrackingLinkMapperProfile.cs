using AutoMapper;

namespace MarketingBox.Reporting.Service.MapperProfiles;


public class TrackingLinkMapperProfile:Profile
{
    public TrackingLinkMapperProfile()
    {
        CreateMap<TrackingLink.Service.Domain.Models.TrackingLink, Domain.Models.TrackingLinks.TrackingLink>();
    }
}