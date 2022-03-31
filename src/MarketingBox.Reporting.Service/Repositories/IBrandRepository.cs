using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models.Reports;

namespace MarketingBox.Reporting.Service.Repositories;

public interface IBrandRepository
{
    Task CreateOrUpdateAsync(IEnumerable<BrandEntity> brandEntities);
    Task DeleteAsync(IEnumerable<BrandEntity> brandEntities);
}