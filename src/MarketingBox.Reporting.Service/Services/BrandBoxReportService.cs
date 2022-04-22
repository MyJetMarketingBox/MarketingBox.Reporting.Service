using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Domain.Models.BrandBox;
using MarketingBox.Affiliate.Service.Grpc.Requests.BrandBox;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Reporting.Service.Services.Interfaces;
using MarketingBox.Sdk.Common.Extensions;

namespace MarketingBox.Reporting.Service.Services;

public class BrandBoxReportService : IBrandBoxReportService
{
    private readonly IBrandBoxService _brandBoxService;

    public BrandBoxReportService(IBrandBoxService brandBoxService)
    {
        _brandBoxService = brandBoxService;
    }

    public async Task<List<long>> GetBrandIdsFromBrandBoxes(List<long> brandBoxIds)
    {
        var box = await _brandBoxService.GetByIdsAsync(new BrandBoxByIdsRequest
            {BrandBoxIds = brandBoxIds});
        var brandBoxes = box.Process() ?? new List<BrandBox>();
        var brandIds = brandBoxes.SelectMany(x => x.BrandIds).Distinct().ToList();
        return brandIds;
    }
}