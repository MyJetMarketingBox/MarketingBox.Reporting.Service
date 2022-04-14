using System.Threading.Tasks;
using ReportTrackingLink = MarketingBox.Reporting.Service.Domain.Models.TrackingLinks.TrackingLink;

namespace MarketingBox.Reporting.Service.Repositories.Interfaces
{
    public interface ITrackingLinkRepository
    {
        Task<ReportTrackingLink> GetAsync(long clickId);
        Task<ReportTrackingLink> CreateOrUpdateAsync(ReportTrackingLink request);
    }
}