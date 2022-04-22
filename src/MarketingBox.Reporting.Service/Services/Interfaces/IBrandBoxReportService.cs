using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketingBox.Reporting.Service.Services.Interfaces;

public interface IBrandBoxReportService
{
    Task<List<long>> GetBrandIdsFromBrandBoxes(List<long> brandBoxIds);
}