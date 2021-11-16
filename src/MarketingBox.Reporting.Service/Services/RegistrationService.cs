using MarketingBox.Reporting.Service.Domain.Extensions;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Models.Common;
using MarketingBox.Reporting.Service.Grpc.Models.Leads;
using MarketingBox.Reporting.Service.Grpc.Models.Leads.Requests;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MarketingBox.Reporting.Service.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILogger<RegistrationService> _logger;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

        public RegistrationService(ILogger<RegistrationService> logger,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
        {
            _logger = logger;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        public async Task<RegistrationSearchResponse> SearchAsync(RegistrationSearchRequest request)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

            try
            {
                var query = context.Registrations.AsQueryable();

                if (!string.IsNullOrEmpty(request.TenantId))
                {
                    query = query.Where(x => x.TenantId == request.TenantId);
                }

                if (request.AffiliateId.HasValue)
                {
                    query = query.Where(x => x.AffiliateId == request.AffiliateId);
                }

                var limit = request.Take <= 0 ? 1000 : request.Take;
                if (request.Asc)
                {
                    if (request.Cursor != null)
                    {
                        query = query.Where(x => x.RegistrationId > request.Cursor);
                    }

                    query = query.OrderBy(x => x.RegistrationId);
                }
                else
                {
                    if (request.Cursor != null)
                    {
                        query = query.Where(x => x.RegistrationId < request.Cursor);
                    }

                    query = query.OrderByDescending(x => x.RegistrationId);
                }

                query = query.Take(limit);

                await query.LoadAsync();

                var response = query
                    .AsEnumerable()
                    .Select(MapToGrpcInner)
                    .ToArray();

                return new RegistrationSearchResponse()
                {
                    Registrations = response
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error happened {@context}", request);

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

        private MarketingBox.Reporting.Service.Grpc.Models.Leads.Registration MapToGrpcInner(Postgres.ReadModels.Leads.Registration registration, int arg2)
        {
            return new MarketingBox.Reporting.Service.Grpc.Models.Leads.Registration()
            {
                RegistrationId = registration.RegistrationId,
                Sequence = registration.Sequence,
                AdditionalInfo = new RegistrationAdditionalInfo()
                {
                    So =   registration.So,
                    Sub =  registration.Sub,
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
                    DepositedAt = registration.DepositDate.HasValue? registration.DepositDate.Value.UtcDateTime : null,
                    ConversionDate = registration.ConversionDate.HasValue ? registration.ConversionDate.Value.UtcDateTime : null,
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
                TenantId = registration.TenantId,
                Status = registration.Status.MapEnum<MarketingBox.Reporting.Service.Domain.Models.Lead.LeadStatus>(),
                UniqueId = registration.UniqueId
            };
        }
    }
}
