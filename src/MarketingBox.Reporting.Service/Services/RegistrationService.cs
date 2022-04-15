using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models.Registrations;
using MarketingBox.Reporting.Service.Grpc.Requests.Registrations;
using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.Grpc;

namespace MarketingBox.Reporting.Service.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILogger<RegistrationService> _logger;
        private readonly DatabaseContextFactory _databaseContextFactory;

        public RegistrationService(ILogger<RegistrationService> logger,
            DatabaseContextFactory databaseContextFactory)
        {
            _logger = logger;
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task<Response<IReadOnlyCollection<RegistrationDetails>>> SearchAsync(RegistrationSearchRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "CustomerReportService.GetCustomersReport receive request : {@Request}",request);

                await using var ctx = _databaseContextFactory.Create();
                IQueryable<RegistrationDetails> query = ctx.RegistrationDetails;

                if (!string.IsNullOrWhiteSpace(request.TenantId))
                    query = query.Where(e => e.TenantId == request.TenantId);
                if (request.AffiliateId.HasValue)
                    query = query.Where(e => e.AffiliateId == request.AffiliateId);
                
                switch (request.Type)
                {
                    case RegistrationsReportType.Registrations:
                        query = query.Where(e => e.Status == RegistrationStatus.Registered);
                        break;
                    case RegistrationsReportType.Ftd:
                        query = query.Where(e => e.Status == RegistrationStatus.Approved || 
                                                 e.Status == RegistrationStatus.Deposited);
                        break;
                    case RegistrationsReportType.All:
                        break;
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

                var result = query.ToList();
                
                return new Response<IReadOnlyCollection<RegistrationDetails>>()
                {
                    Status = ResponseStatus.Ok,
                    Data = result,
                    Total = total
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error happened for request {@context}", request);
                return ex.FailedResponse<IReadOnlyCollection<RegistrationDetails>>();
            }
        }
    }
}
