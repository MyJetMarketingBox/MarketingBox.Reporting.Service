using System;
using System.Collections.Generic;
using System.Linq;
using MarketingBox.Reporting.Service.Grpc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Repositories;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.Grpc;
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

        public async Task<Response<IReadOnlyCollection<Report>>> SearchAsync(ReportSearchRequest request)
        {
            try
            {
                var result = await _repository.SearchAsync(request);
                
                return new Response<IReadOnlyCollection<Report>>()
                {
                    Status = ResponseStatus.Ok,
                    Data =  result.ToList()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error happened {@context}", request);

                return e.FailedResponse<IReadOnlyCollection<Report>>();
            }
        }
    }
}