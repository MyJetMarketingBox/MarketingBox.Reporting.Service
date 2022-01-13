using MarketingBox.Reporting.Service.Domain.Extensions;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Models.Common;
using MarketingBox.Reporting.Service.Grpc.Models.Leads;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MarketingBox.Reporting.Service.Domain.Registrations;
using MarketingBox.Reporting.Service.Grpc.Models.Registrations.Requests;

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

        public async Task<RegistrationSearchResponse> SearchAsync(RegistrationSearchRequest request)
        {
            try
            {
                _logger.LogInformation(
                    $"CustomerReportService.GetCustomersReport receive request : {JsonConvert.SerializeObject(request)}");

                //if (!request.MasterAffiliateId.HasValue)
                //{
                //    return new RegistrationSearchResponse()
                //    {
                //        Error = new Error()
                //        {
                //            Message = "Cannot get registrations without master affiliate id.",
                //            Type = ErrorType.Unknown
                //        }
                //    };
                //}
                
                await using var ctx = _databaseContextFactory.Create();

                IQueryable<Postgres.ReadModels.Registrations.Registration> query = ctx.Registrations;

                if (!string.IsNullOrWhiteSpace(request.TenantId))
                    query = query.Where(e => e.TenantId == request.TenantId);

                if (request.AffiliateId.HasValue)
                    query = query.Where(e => e.AffiliateId == request.AffiliateId);
                
                if (request.MasterAffiliateId.HasValue)
                {
                    var affiliateAccesses = ctx.AffiliateAccesses
                        .Where(e => e.MasterAffiliateId == request.MasterAffiliateId)
                        .ToList();

                    if (affiliateAccesses.Any()) 
                    { 
                        query = query.Where(e => e.AffiliateId == request.MasterAffiliateId
                                                 || affiliateAccesses.Select(x => x.AffiliateId)
                                                     .Contains(e.AffiliateId));
                    }
                    else
                    {
                        query = query.Where(e => e.AffiliateId == request.MasterAffiliateId);
                    }
                }
                
                query = query.Where(e => e.RegistrationId < request.Cursor);
                
                query = request.Asc 
                    ? query.OrderBy(e => e.RegistrationId) 
                    : query.OrderByDescending(e => e.RegistrationId);

                query = query.Take(request.Take <= 0 ? 1000 : request.Take);
                
                var response = query
                    .ToList()
                    .Select(MapToGrpcInner)
                    .ToArray();

                return new RegistrationSearchResponse()
                {
                    Registrations = response
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

        private static MarketingBox.Reporting.Service.Grpc.Models.Leads.Registration MapToGrpcInner(Postgres.ReadModels.Registrations.Registration registration, int arg2)
        {
            return new MarketingBox.Reporting.Service.Grpc.Models.Leads.Registration()
            {
                RegistrationId = registration.RegistrationId,
                Sequence = registration.Sequence,
                AdditionalInfo = new RegistrationAdditionalInfo()
                {
                    So = registration.So,
                    Sub = registration.Sub,
                    Sub1 = registration.Sub1,
                    Sub10 =registration.Sub10,
                    Sub2 = registration.Sub2,
                    Sub3 = registration.Sub3,
                    Sub4 = registration.Sub4,
                    Sub5 = registration.Sub5,
                    Sub6 = registration.Sub6,
                    Sub7 = registration.Sub7,
                    Sub8 = registration.Sub8,
                    Sub9 = registration.Sub9,
                },
                //CrmStatus = registration.CrmStatus.MapEnum<MarketingBox.Reporting.Service.Domain.Models.Registration.LeadCrmStatus>(),
                GeneralInfo = new RegistrationGeneralInfo()
                {
                    CreatedAt = registration.CreatedAt.UtcDateTime,
                    DepositedAt = registration.DepositDate?.UtcDateTime,
                    ConversionDate = registration.ConversionDate?.UtcDateTime,
                    Country = registration.Country,
                    Email = registration.Email,
                    FirstName = registration.FirstName,
                    Ip = registration.Ip,
                    LastName = registration.LastName,
                    Phone = registration.Phone
                },
                RouteInfo = new RegistrationRouteInfo()
                {
                    AffiliateId = registration.AffiliateId,
                    BrandId = registration.BrandId,
                    CampaignId = registration.CampaignId,
                    IntegrationId = registration.IntegrationId,
                },
                CrmStatus = registration.CrmStatus,
                TenantId = registration.TenantId,
                Status = registration.Status.MapEnum<RegistrationStatus>(),
                UniqueId = registration.UniqueId
            };
        }
    }
}
