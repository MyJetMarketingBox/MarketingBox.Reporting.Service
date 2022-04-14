using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Reporting.Service.Repositories.Interfaces;
using MarketingBox.Sdk.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using ReportTrackingLink = MarketingBox.Reporting.Service.Domain.Models.TrackingLinks.TrackingLink;

namespace MarketingBox.Reporting.Service.Repositories
{
    public class TrackingLinkRepository : ITrackingLinkRepository
    {
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

        public TrackingLinkRepository(DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        public async Task<ReportTrackingLink> GetAsync(long clickId)
        {
            await using var ctx = new DatabaseContext(_dbContextOptionsBuilder.Options);

            var trackingLink = await ctx.TrackingLinks.FirstOrDefaultAsync(x => x.ClickId == clickId);

            if (trackingLink is null)
            {
                throw new NotFoundException($"Tracking link with {nameof(clickId)}", clickId);
            }

            return trackingLink;
        }

        public async Task<ReportTrackingLink> CreateOrUpdateAsync(
            ReportTrackingLink request)
        {
            await using var ctx = new DatabaseContext(_dbContextOptionsBuilder.Options);

            var rows = await ctx.TrackingLinks.Upsert(request).RunAsync();

            if (rows == 0)
            {
                throw new NotFoundException($"Tracking link with {nameof(request.ClickId)}", request.ClickId);
            }

            return request;
        }
    }
}