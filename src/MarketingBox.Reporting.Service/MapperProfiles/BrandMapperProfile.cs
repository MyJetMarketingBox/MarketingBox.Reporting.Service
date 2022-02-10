using AutoMapper;
using MarketingBox.Affiliate.Service.MyNoSql.Brands;
using MarketingBox.Reporting.Service.Domain.Models.Reports;

namespace MarketingBox.Reporting.Service.MapperProfiles;

public class BrandMapperProfile:Profile
{
    public BrandMapperProfile()
    {
        CreateMap<BrandNoSql, BrandEntity>();
    }
}