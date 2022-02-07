using DotNetCoreDecorators;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Autofac;
using MarketingBox.Registration.Service.Messages.Registrations;
using MarketingBox.Reporting.Service.Engines;

namespace MarketingBox.Reporting.Service.Subscribers
{
    public class RegistrationUpdateMessageSubscriber : IStartable
    {
        private readonly ILogger<RegistrationUpdateMessageSubscriber> _logger;
        private readonly IReportingEngine _reportingEngine;

        public RegistrationUpdateMessageSubscriber(
            ISubscriber<RegistrationUpdateMessage> subscriber,
            ILogger<RegistrationUpdateMessageSubscriber> logger,
            IReportingEngine reportingEngine)
        {
            _logger = logger;
            _reportingEngine = reportingEngine;
            subscriber.Subscribe(Consume);
        }

        private async ValueTask Consume(RegistrationUpdateMessage message)
        {
            _logger.LogInformation("Consuming message {@context}", message);

            try
            {
                await _reportingEngine.ProcessMessageAsync(message);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "Error during consumptions {@context}", message);
                throw;
            }

            _logger.LogInformation("Has been consumed {@context}", message);
        }


        public void Start()
        {
        }
    }
}
