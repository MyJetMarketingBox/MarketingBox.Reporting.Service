using System;
using System.Linq;
using MarketingBox.Reporting.Service.Grpc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Reporting.Service.Grpc.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Models.Reports.Requests;
using MarketingBox.Reporting.Service.Grpc.Models.Common;
using MarketingBox.Reporting.Service.Repositories;
using ReportSearchRequest = MarketingBox.Reporting.Service.Grpc.Models.Reports.Requests.ReportSearchRequest;

namespace MarketingBox.Reporting.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;
        private readonly IRegistrationDetailsRepository _repository;
        private readonly IMapper _mapper;

        public ReportService(ILogger<ReportService> logger,
            IRegistrationDetailsRepository repository,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReportSearchResponse> SearchAsync(ReportSearchRequest request)
        {
            try
            {
                var result = await _repository.SearchAsync(
                    _mapper.Map<MarketingBox.Reporting.Service.Domain.Models.Reports.ReportSearchRequest>(request));
                return new ReportSearchResponse
                {
                    Reports =  result.Select(x => _mapper.Map<Report>(x)).ToList()
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

        public async Task<ReportByDaySearchResponse> SearchByDayAsync(ReportByDaySearchRequest request)
        {
            try
            {
                var result = _repository.SearchByDateAsync();

                return new ReportByDaySearchResponse()
                {
                    Reports = ArraySegment<ReportByDay>.Empty
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error happened {@context}", request);

                return new ReportByDaySearchResponse()
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