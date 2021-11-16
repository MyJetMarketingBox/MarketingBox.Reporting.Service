using DotNetCoreDecorators;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.EntityFrameworkCore;

namespace MarketingBox.Reporting.Service.Subscribers
{
    public class AffiliateAccessUpdateMessageSubscriber
    {
        private readonly ILogger<AffiliateAccessUpdateMessageSubscriber> _logger;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

        public AffiliateAccessUpdateMessageSubscriber(
            ISubscriber<MarketingBox.Affiliate.Service.Messages.AffiliateAccesses.AffiliateAccessUpdated> subscriber,
            ILogger<AffiliateAccessUpdateMessageSubscriber> logger,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
        {
            _logger = logger;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            subscriber.Subscribe(Consume);
        }

        private async ValueTask Consume(MarketingBox.Affiliate.Service.Messages.AffiliateAccesses.AffiliateAccessUpdated message)
        {
            _logger.LogInformation("Consuming message {@context}", message);
            
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

            var registration = MapToReadModel(message);

            try
            {
                var affectedRowsCount = await context.AffiliateAccesses
                    .Upsert(registration)
                    .RunAsync();

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

        private static Postgres.ReadModels.AffiliateAccesses.AffiliateAccess MapToReadModel(MarketingBox.Affiliate.Service.Messages.AffiliateAccesses.AffiliateAccessUpdated message)
        {
            return new Postgres.ReadModels.AffiliateAccesses.AffiliateAccess()
            {
                AffiliateId = message.AffiliateId,
                MasterAffiliateId = message.MasterAffiliateId,
                Id = message.Id
            };
        }
    }
}
