using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models.Registrations;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Requests.Registrations;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Exceptions;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.Grpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
                    "AffiliateReportService.GetRegistrations receive request : {@Request}", request);

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

                var total = query.Count();
                if (request.Asc)
                {
                    if (request.Cursor.HasValue)
                    {
                        query = query.Where(x => x.RegistrationId > request.Cursor);
                    }

                    query = query.OrderBy(x => x.RegistrationId);
                }
                else
                {
                    if (request.Cursor.HasValue)
                    {
                        query = query.Where(x => x.RegistrationId < request.Cursor);
                    }

                    query = query.OrderByDescending(x => x.RegistrationId);
                }

                if (request.Take.HasValue)
                {
                    query = query.Take(request.Take.Value);
                }

                await query.LoadAsync();

                var registrations = query.ToList();

                return new Response<IReadOnlyCollection<RegistrationDetails>>
                {
                    Status = ResponseStatus.Ok,
                    Data = registrations,
                    Total = total
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error happened for {@context} with message {@message}", request, ex.Message);
                return ex.FailedResponse<IReadOnlyCollection<RegistrationDetails>>();
            }
        }

        public async Task<Response<RegistrationDetails>> GetRegistration(RegistrationByAffiliateRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "AffiliateReportService.GetRegistration receive request : {@Request}", request);

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