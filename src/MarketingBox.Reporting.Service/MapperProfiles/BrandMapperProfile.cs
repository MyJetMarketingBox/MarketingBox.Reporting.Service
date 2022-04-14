using AutoMapper;
using MarketingBox.Affiliate.Service.MyNoSql.Brands;
using MarketingBox.Reporting.Service.Domain.Models.Reports;

namespace MarketingBox.Reporting.Service.MapperProfiles;

public class BrandMapperProfile : Profile
{
    public BrandMapperProfile()
    {
        //CreateMap<BrandNoSql, BrandEntity>();
        //     .ForMember(x => x.PayoutAmount,
        //         x => x.MapFrom(z => z.Payout.Amount))
        //     .ForMember(x => x.PayoutCurrency,
        //         x => x.MapFrom(z => z.Payout.Currency))
        //     .ForMember(x => x.PayoutPlan,
        //         x => x.MapFrom(z => z.Payout.Plan))
        //     .ForMember(x => x.RevenueAmount,
        //         x => x.MapFrom(z => z.Revenue.Amount))
        //     .ForMember(x => x.RevenueCurrency,
        //         x => x.MapFrom(z => z.Revenue.Currency))
        //     .ForMember(x => x.RevenuePlan,
        //         x => x.MapFrom(z => z.Revenue.Plan));
        // CreateMap<MarketingBox.Affiliate.Service.Domain.Models.Common.Currency, Currency>();
        // CreateMap<MarketingBox.Affiliate.Service.Domain.Models.Common.PayoutType, Plan>();
    }
}