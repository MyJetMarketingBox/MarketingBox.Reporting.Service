using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Models.Common;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Domain.Models.Affiliates;
using MarketingBox.Affiliate.Service.Grpc.Models.Affiliates.Requests;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Grpc.Models.Registrations;
using Newtonsoft.Json;
using MarketingBox.Reporting.Service.Grpc.Models.Registrations.Requests;
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

        public async Task<RegistrationSearchResponse> SearchAsync(RegistrationSearchRequest request)
        {
            try
            {
                _logger.LogInformation(
                    $"CustomerReportService.GetCustomersReport receive request : {JsonConvert.SerializeObject(request)}");

                await using var ctx = _databaseContextFactory.Create();
                IQueryable<RegistrationDetails> query = ctx.RegistrationDetails;
                
                if (request.MasterAffiliateId.HasValue)
                {
                    var master = await _affiliateService.GetAsync(new AffiliateGetRequest()
                    {
                        AffiliateId = (long)request.MasterAffiliateId
                    });

                    if (master.Affiliate == null || master.Error != null)
                    {
                        var message = $"Cannot find masterAffiliate by id {request.MasterAffiliateId}";
                        _logger.LogError(message);
                        return new RegistrationSearchResponse()
                        {
                            Error = new Error()
                            {
                                Message = message,
                                Type = ErrorType.InvalidParameter
                            }
                        };
                    }
                    
                    var assignAffiliates = GetAffiliateIdListByAccessTable(ctx, master.Affiliate.AffiliateId);
                    
                    switch (master.Affiliate.GeneralInfo.Role)
                    {
                        case AffiliateRole.Affiliate:
                            query = query.Where(e => e.AffiliateId == master.Affiliate.AffiliateId);
                            break;
                        case AffiliateRole.AffiliateManager:
                            query = query.Where(e => assignAffiliates.Contains(e.AffiliateId));
                            break;
                        case AffiliateRole.IntegrationManager:
                            break;
                        case AffiliateRole.MasterAffiliate:
                            query = query.Where(e => assignAffiliates.Contains(e.AffiliateId));
                            break;
                        case AffiliateRole.MasterAffiliateReferral:
                            query = query.Where(e => assignAffiliates.Contains(e.AffiliateId));
                            break;
                        default:
                            break;
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
                        query = query.Where(e => e.Status == RegistrationStatus.Approved);
                        break;
                    case RegistrationsReportType.All:
                        break;
                    default:
                        break;
                }
                
                if (request.Cursor.HasValue)
                    query = query.Where(e => e.RegistrationId < request.Cursor);
                query = request.Asc 
                    ? query.OrderBy(e => e.RegistrationId) 
                    : query.OrderByDescending(e => e.RegistrationId);
                query = query.Take(request.Take <= 0 ? 1000 : request.Take);

                return new RegistrationSearchResponse()
                {
                    Registrations = query.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error happened {@context}", request);
                return new RegistrationSearchResponse()
                {
                    Error = new Error()
                    {
                        Message = "Internal error happened",
                        Type = ErrorType.Unknown
                    }
                };
            }
        }

        private static IEnumerable<long> GetAffiliateIdListByAccessTable(DatabaseContext ctx, long masterId)
        {
            var arr = new List<long>
            {
                masterId
            };
            var affiliateAccesses = ctx.AffiliateAccesses
                .Where(e => e.MasterAffiliateId == masterId)
                .ToList();
            affiliateAccesses.ForEach(e => arr.Add(e.AffiliateId));
            return arr;
        }
    }
}
