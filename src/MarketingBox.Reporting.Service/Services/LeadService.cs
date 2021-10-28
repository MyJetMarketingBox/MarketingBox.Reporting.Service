using MarketingBox.Reporting.Service.Domain.Extensions;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Models.Common;
using MarketingBox.Reporting.Service.Grpc.Models.Leads;
using MarketingBox.Reporting.Service.Grpc.Models.Leads.Requests;
using MarketingBox.Reporting.Service.Grpc.Models.Reports.Requests;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MarketingBox.Reporting.Service.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILogger<LeadService> _logger;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

        public LeadService(ILogger<LeadService> logger,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
        {
            _logger = logger;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        public async Task<LeadSearchResponse> SearchAsync(LeadSearchRequest request)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

            try
            {
                var query = context.Leads.AsQueryable();

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
                        query = query.Where(x => x.LeadId > request.Cursor);
                    }

                    query = query.OrderBy(x => x.LeadId);
                }
                else
                {
                    if (request.Cursor != null)
                    {
                        query = query.Where(x => x.LeadId < request.Cursor);
                    }

                    query = query.OrderByDescending(x => x.LeadId);
                }

                query = query.Take(limit);

                await query.LoadAsync();

                var response = query
                    .AsEnumerable()
                    .Select(MapToGrpcInner)
                    .ToArray();

                return new LeadSearchResponse()
                {
                    Leads = response
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error happened {@context}", request);

                return new LeadSearchResponse()
                {
                    Error = new Error()
                    {
                        Message = "Internal error happened",
                        Type = ErrorType.Unknown
                    }
                };
            }
        }

        private MarketingBox.Reporting.Service.Grpc.Models.Leads.Lead MapToGrpcInner(Postgres.ReadModels.Leads.Lead lead, int arg2)
        {
            return new MarketingBox.Reporting.Service.Grpc.Models.Leads.Lead()
            {
                LeadId = lead.LeadId,
                Sequence = lead.Sequence,
                AdditionalInfo = new LeadAdditionalInfo()
                {
                    So =   lead.So,
                    Sub =  lead.Sub,
                    Sub1 = lead.Sub1,
                    Sub10 =lead.Sub10,
                    Sub2 = lead.Sub2,
                    Sub3 = lead.Sub3,
                    Sub4 = lead.Sub4,
                    Sub5 = lead.Sub5,
                    Sub6 = lead.Sub6,
                    Sub7 = lead.Sub7,
                    Sub8 = lead.Sub8,
                    Sub9 = lead.Sub9,
                },
                //CrmStatus = lead.CrmStatus.MapEnum<MarketingBox.Reporting.Service.Domain.Models.Lead.LeadCrmStatus>(),
                GeneralInfo = new LeadGeneralInfo()
                {
                    CreatedAt = lead.CreatedAt.UtcDateTime,
                    Email = lead.Email,
                    FirstName = lead.FirstName,
                    Ip = lead.Ip,
                    LastName = lead.LastName,
                    Phone = lead.Phone
                },
                RouteInfo = new LeadRouteInfo()
                {
                    AffiliateId = lead.AffiliateId,
                    CampaignId = lead.CampaignId,
                    BoxId = lead.BoxId,
                    BrandId = lead.BrandId,
                },
                TenantId = lead.TenantId,
                Status = lead.Status.MapEnum<MarketingBox.Reporting.Service.Domain.Models.Lead.LeadStatus>(),
                UniqueId = lead.UniqueId
            };
        }
    }
}
