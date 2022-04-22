using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Brands;
using MarketingBox.Reporting.Service.Domain.Models.Brands;

namespace MarketingBox.Reporting.Service.MapperProfiles;

public class BrandMapperProfile : Profile
{
    public BrandMapperProfile()
    {
        CreateMap<BrandMessage, BrandEntity>();
    }
}