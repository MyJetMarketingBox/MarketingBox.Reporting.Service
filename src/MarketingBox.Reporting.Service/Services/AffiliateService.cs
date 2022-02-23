using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Models.RegistrationsByAffiliate;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Sdk.Common.Exceptions;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.Grpc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarketingBox.Reporting.Service.Services
{
    public class AffiliateService : IAffiliateService
    {
        private readonly ILogger<AffiliateService> _logger;
        private readonly DatabaseContextFactory _databaseContextFactory;

        public AffiliateService(ILogger<AffiliateService> logger,
            DatabaseContextFactory databaseContextFactory)
        {
            _logger = logger;
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task<Response<IReadOnlyCollection<RegistrationDetails>>> GetRegistrations(
            RegistrationsByAffiliateRequest request)
        {
            try
            {
                _logger.LogInformation(
                    $"AffiliateReportService.GetRegistrations receive request : {JsonConvert.SerializeObject(request)}");

                await using var ctx = _databaseContextFactory.Create();
                var query = ctx.RegistrationDetails
                    .Where(e => e.CreatedAt >= request.From &&
                                e.CreatedAt <= request.To &&
                                e.TenantId == request.TenantId &&
                                e.AffiliateId == request.AffiliateId);
                switch (request.Type)
                {
                    case RegistrationsReportType.Registrations:
                        query = query.Where(e => e.Status == RegistrationStatus.Registered);
                        break;
                    case RegistrationsReportType.Ftd:
                        query = query.Where(e => e.Status == RegistrationStatus.Approved);
                        break;
                    case RegistrationsReportType.All:
                        break;
                    default:
                        throw new Exception("Something wrong with GetRegistrations at switch construction.");
                }

                var registrations = query.ToList();
                if (!registrations.Any())
                {
                    throw new NotFoundException("There is no entity for such request.");
                }

                return new Response<IReadOnlyCollection<RegistrationDetails>>
                {
                    Status = ResponseStatus.Ok,
                    Data = registrations
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error happened {@context} {@message}", request, ex.Message);
                return ex.FailedResponse<IReadOnlyCollection<RegistrationDetails>>();
            }
        }

        public async Task<Response<RegistrationDetails>> GetRegistration(RegistrationByAffiliateRequest request)
        {
            try
            {
                _logger.LogInformation(
                    $"AffiliateReportService.GetRegistration receive request : {JsonConvert.SerializeObject(request)}");

                await using var ctx = _databaseContextFactory.Create();

                var registration = ctx.RegistrationDetails.FirstOrDefault(
                    e => e.RegistrationUid == request.RegistrationUid &&
                         e.AffiliateId == request.AffiliateId &&
                         e.TenantId == request.TenantId);

                if (registration is null)
                {
                    throw new NotFoundException(nameof(request.AffiliateId), request.AffiliateId);
                }
                return new Response<RegistrationDetails>()
                {
                    Status = ResponseStatus.Ok,
                    Data = registration
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error happened {@context} {@message}", request, ex.Message);
                return ex.FailedResponse<RegistrationDetails>();
            }
        }
    }
}