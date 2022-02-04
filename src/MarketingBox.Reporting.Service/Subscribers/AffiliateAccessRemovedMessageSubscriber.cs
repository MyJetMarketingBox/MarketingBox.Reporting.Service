using DotNetCoreDecorators;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace MarketingBox.Reporting.Service.Subscribers
{
    public class AffiliateAccessRemovedMessageSubscriber
    {
        private readonly ILogger<AffiliateAccessRemovedMessageSubscriber> _logger;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

        public AffiliateAccessRemovedMessageSubscriber(
            ISubscriber<MarketingBox.Affiliate.Service.Messages.AffiliateAccesses.AffiliateAccessRemoved> subscriber,
            ILogger<AffiliateAccessRemovedMessageSubscriber> logger,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
        {
            _logger = logger;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            subscriber.Subscribe(Consume);
        }

        private async ValueTask Consume(MarketingBox.Affiliate.Service.Messages.AffiliateAccesses.AffiliateAccessRemoved message)
        {
            _logger.LogInformation("Consuming message {@context}", message);
            
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

            try
            {
                var affectedRowsCount = await context.AffiliateAccesses
                    .Where(x => x.MasterAffiliateId == message.MasterAffiliateId &&
                                x.AffiliateId == message.AffiliateId)
                    .DeleteFromQueryAsync();

                if (affectedRowsCount != 1)
                {
                    _logger.LogInformation("There is nothing to update: {@context}", message);
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "Error during consumptions {@context}", message);

                throw;
            }

            _logger.LogInformation("Has been consumed {@context}", message);
        }
    }
}
