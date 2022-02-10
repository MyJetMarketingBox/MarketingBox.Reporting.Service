using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Domain.Models.Reports.Requests;

namespace MarketingBox.Reporting.Service.Repositories
{
    public interface IRegistrationDetailsRepository
    {
        Task SaveAsync(RegistrationDetails entity);
        Task<IEnumerable<Report>> SearchAsync(ReportSearchRequest request);
    }
}