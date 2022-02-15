using System;
using System.Linq;
using MarketingBox.Reporting.Service.Grpc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Grpc.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Models.Common;
using MarketingBox.Reporting.Service.Repositories;
using ReportSearchRequest = MarketingBox.Reporting.Service.Domain.Models.Reports.Requests.ReportSearchRequest;

namespace MarketingBox.Reporting.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;
        private readonly IRegistrationDetailsRepository _repository;
        public ReportService(ILogger<ReportService> logger,
            IRegistrationDetailsRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<ReportSearchResponse> SearchAsync(ReportSearchRequest request)
        {
            try
            {
                var result = await _repository.SearchAsync(request);
                return new ReportSearchResponse
                {
                    Reports =  result.ToList()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error happened {@context}", request);

                return new ReportSearchResponse()
                {
                    Error = new Error()
                    {
                        Message = "Internal error happened",
                        Type = ErrorType.Unknown
                    }
                };
            }
        }
    }
}