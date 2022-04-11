using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models;
using Newtonsoft.Json;
using MarketingBox.Reporting.Service.Grpc.Models.Registrations.Requests;
using MarketingBox.Sdk.Common.Exceptions;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.Grpc;
using IAffiliateService = MarketingBox.Affiliate.Service.Grpc.IAffiliateService;

namespace MarketingBox.Reporting.Service.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILogger<RegistrationService> _logger;
        private readonly DatabaseContextFactory _databaseContextFactory;
        private readonly IAffiliateService _affiliateService;

        public RegistrationService(ILogger<RegistrationService> logger,
            DatabaseContextFactory databaseContextFactory, 
            IAffiliateService affiliateService)
        {
            _logger = logger;
            _databaseContextFactory = databaseContextFactory;
            _affiliateService = affiliateService;
        }

        public async Task<Response<IReadOnlyCollection<RegistrationDetails>>> SearchAsync(RegistrationSearchRequest request)
        {
            try
            {
                _logger.LogInformation(
                    $"CustomerReportService.GetCustomersReport receive request : {JsonConvert.SerializeObject(request)}");

                await using var ctx = _databaseContextFactory.Create();
                IQueryable<RegistrationDetails> query = ctx.RegistrationDetails;
                
                if (request.MasterAffiliateId.HasValue)
                {
                    var master = await _affiliateService.GetAsync(new ()
                    {
                        AffiliateId = (long)request.MasterAffiliateId
                    });
                    if (master.Status!=ResponseStatus.Ok)
                    {
                        return new Response<IReadOnlyCollection<RegistrationDetails>>
                        {
                            Error = master.Error,
                            Status = master.Status
                        };
                    }
                }
                
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
                    default:
                        break;
                }
                
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
                query = query.Take(request.Take <= 0 ? 1000 : request.Take);

                var result = query.ToList();
                if (!result.Any())
                {
                    throw new NotFoundException("There is no entity for such request.");
                }
                
                return new Response<IReadOnlyCollection<RegistrationDetails>>()
                {
                    Status = ResponseStatus.Ok,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error happened {@context}", request);
                return ex.FailedResponse<IReadOnlyCollection<RegistrationDetails>>();
            }
        }
    }
}
