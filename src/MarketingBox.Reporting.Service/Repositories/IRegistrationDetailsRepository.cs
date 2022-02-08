using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Domain.Models.Reports;

namespace MarketingBox.Reporting.Service.Repositories
{
    public interface IRegistrationDetailsRepository
    {
        Task SaveAsync(RegistrationDetails entity);
        Task<IEnumerable<AggregatedReportEntity>> SearchAsync(ReportSearchRequest request);
        Task SearchByDateAsync();
    }
}