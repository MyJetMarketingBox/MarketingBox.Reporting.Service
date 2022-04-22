using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models.Registrations;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Requests.Reports;

namespace MarketingBox.Reporting.Service.Repositories.Interfaces
{
    public interface IRegistrationDetailsRepository
    {
        Task SaveAsync(RegistrationDetails entity);
        Task<(List<Report>, int)> SearchAsync(ReportSearchRequest request);
    }
}