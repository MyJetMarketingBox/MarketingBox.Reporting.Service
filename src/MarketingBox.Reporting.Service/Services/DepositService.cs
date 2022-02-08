using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Models.Common;
using MarketingBox.Reporting.Service.Grpc.Models.Deposits;
using MarketingBox.Reporting.Service.Grpc.Models.Deposits.Requests;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MarketingBox.Reporting.Service.Services
{
    public class DepositService : IDepositService
    {
        private readonly ILogger<DepositService> _logger;

        public DepositService(ILogger<DepositService> logger)
        {
            _logger = logger;
        }

        public async Task<DepositSearchResponse> SearchAsync(DepositSearchRequest request)
        {
            var message = $"IDepositService receive request {JsonConvert.SerializeObject(request)}";
            _logger.LogError(message);

            return new DepositSearchResponse()
            {
                Error = new Error()
                {
                    Message = "IDepositService is deprecated. Use IRegistrationService.",
                    Type = ErrorType.Unknown
                }
            };
        }
    }
}
