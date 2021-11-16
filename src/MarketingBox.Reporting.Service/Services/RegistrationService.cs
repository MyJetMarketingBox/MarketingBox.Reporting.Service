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
using Dapper;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Deposits;

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
            var sorting = request.Asc ? "ASC" : "DESC";
            var access = "";
            var where = "";

            if (request.AffiliateId.HasValue)
            {
                where += $@" and d.""AffiliateId"" = @AffiliateId ";
            }

            if (request.MasterAffiliateId.HasValue)
            {
                access += $@" INNER JOIN ""reporting-service"".affiliate_access as acc ON 
                r.""AffiliateId"" = acc.""AffiliateId"" and acc.""MasterAffiliateId"" = @MasterAffiliateId ";
            }

            var searchQuery = $@"
            SELECT 
            r.""RegistrationId"",
            r.""AffiliateId"", 
            r.""BrandId"",
            r.""CampaignId"", 
            r.""ConversionDate"", 
            r.""Country"",
            r.""CreatedAt"",
            r.""CrmStatus"",
            r.""DepositDate"", 
            r.""Email"",
            r.""FirstName"",
            r.""IntegrationId"", 
            r.""Ip"",
            r.""LastName"",
            r.""Phone"", 
            r.""Sequence"", 
            r.""So"",
            r.""Status"",
            r.""Sub"",
            r.""Sub1"",
            r.""Sub10"", 
            r.""Sub2"",
            r.""Sub3"",
            r.""Sub4"",
            r.""Sub5"",
            r.""Sub6"",
            r.""Sub7"",
            r.""Sub8"",
            r.""Sub9"",
            r.""TenantId"",
            r.""UniqueId"",
            r.""UpdatedAt""
            FROM ""reporting-service"".registrations AS r
            {access}
            WHERE r.""TenantId"" = @TenantId
            {where}
            ORDER BY r.""RegistrationId"" {sorting}
            LIMIT @Limit";

            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            var limit = request.Take <= 0 ? 1000 : request.Take;

            try
            {
                var array = await context.Database.GetDbConnection()
                    .QueryAsync<Postgres.ReadModels.Leads.Registration>(searchQuery, new
                    {
                        MasterAffiliateId = request.MasterAffiliateId ?? 0,
                        TenantId = request.TenantId,
                        FromId = request.Cursor ?? 0,
                        Limit = limit,
                        AffiliateId = request.AffiliateId ?? 0,
                    });

                var response = array
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
