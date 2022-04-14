using System;
using System.Collections.Generic;
using System.Linq;
using MarketingBox.Reporting.Service.Grpc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using FluentValidation;
using MarketingBox.Affiliate.Service.Client;
using MarketingBox.Affiliate.Service.Domain.Models.Country;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Domain.Models.Enums;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Repositories;
using MarketingBox.Reporting.Service.Repositories.Interfaces;
using MarketingBox.Sdk.Common.Exceptions;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.Grpc;
using ReportSearchRequest = MarketingBox.Reporting.Service.Grpc.Requests.Reports.ReportSearchRequest;

namespace MarketingBox.Reporting.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;
        private readonly IRegistrationDetailsRepository _repository;
        private readonly ICountryClient _countryClient;
        private readonly IValidator<ReportSearchRequest> _validator;

        public ReportService(
            ILogger<ReportService> logger,
            IRegistrationDetailsRepository repository,
            ICountryClient countryClient,
            IValidator<ReportSearchRequest> validator)
        {
            _logger = logger;
            _repository = repository;
            _countryClient = countryClient;
            _validator = validator;
        }

        public async Task<Response<IReadOnlyCollection<Report>>> SearchAsync(ReportSearchRequest request)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(request);
                
                if (!string.IsNullOrEmpty(request.CountryCode) && request.CountryCodeType.HasValue)
                {
                    var country = await GetCountry(request.CountryCodeType.Value,request.CountryCode.ToUpper());
                    request.CountryCode = country.Alfa2Code;
                }

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
        
        private async Task<Country> GetCountry(CountryCodeType countryCodeType, string countryCode)
        {
            var countries = await _countryClient.GetCountries();
            var country = countryCodeType switch
            {
                CountryCodeType.Numeric => countries.FirstOrDefault(x => x.Numeric == countryCode),
                CountryCodeType.Alfa2Code => countries.FirstOrDefault(x => x.Alfa2Code == countryCode),
                CountryCodeType.Alfa3Code => countries.FirstOrDefault(x => x.Alfa3Code == countryCode),
                _ => throw new ArgumentOutOfRangeException(nameof(countryCodeType), countryCodeType, null)
            };
            if (country is null)
            {
                throw new NotFoundException($"Country with code {countryCodeType}", countryCode);
            }

            return country;
        }
    }
}