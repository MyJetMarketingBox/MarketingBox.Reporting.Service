using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Models.Common;
using MarketingBox.Reporting.Service.Grpc.Models.Deposits;
using MarketingBox.Reporting.Service.Grpc.Models.Deposits.Requests;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MarketingBox.Reporting.Service.Domain.Extensions;
using Deposit = MarketingBox.Reporting.Service.Postgres.ReadModels.Deposits.Deposit;
using MarketingBox.Reporting.Service.Domain.Crm;

namespace MarketingBox.Reporting.Service.Services
{
    public class DepositService : IDepositService
    {
        private readonly ILogger<DepositService> _logger;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

        public DepositService(ILogger<DepositService> logger,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
        {
            _logger = logger;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        public async Task<DepositSearchResponse> SearchAsync(DepositSearchRequest request)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

            var sorting = request.Asc ? "ASC" : "DESC";
            var access = "";
            var where = "";
            var tenantWhere = "";

            if (request.AffiliateId.HasValue)
            {
                where += $@" and d.""AffiliateId"" = @AffiliateId ";
            }

            if (request.MasterAffiliateId.HasValue)
            {
                access += $@" INNER JOIN ""reporting-service"".affiliate_access as acc ON 
                d.""AffiliateId"" = acc.""AffiliateId"" and acc.""MasterAffiliateId"" = @MasterAffiliateId ";
            }

            if (request.RegistrationId.HasValue)
            {
                where += $@" and d.""RegistrationId"" = @RegistrationId ";
            }

            if (!string.IsNullOrWhiteSpace(request.TenantId))
            {
                tenantWhere = $@" and d.""TenantId"" = @TenantId ";
            }

            var searchQuery = string.Empty;
            if (request.Asc)
            {
                searchQuery = $@"
                    SELECT 
                    d.""RegistrationId"", 
                    d.""AffiliateId"", 
                    d.""BrandId"", 
                    d.""CrmStatus"", 
                    d.""CampaignId"", 
                    d.""ConversionDate"", 
                    d.""Country"", 
                    d.""CreatedAt"", 
                    d.""CustomerId"", 
                    d.""Email"", 
                    d.""IntegrationId"", 
                    d.""RegisterDate"", 
                    d.""Sequence"", 
                    d.""TenantId"", 
                    d.""UpdateMode"", 
                    d.""Status"", 
                    d.""UniqueId""
                    FROM ""reporting-service"".deposits AS d
                    {access}
                    WHERE d.""RegistrationId"" > @FromId
                    {tenantWhere}
                    {where}
                    ORDER BY d.""RegistrationId"" {sorting}
                    LIMIT @Limit";
            }
            else
            {
                searchQuery = $@"
                    SELECT 
                    d.""RegistrationId"", 
                    d.""AffiliateId"", 
                    d.""BrandId"", 
                    d.""CrmStatus"", 
                    d.""CampaignId"", 
                    d.""ConversionDate"", 
                    d.""Country"", 
                    d.""CreatedAt"", 
                    d.""CustomerId"", 
                    d.""Email"", 
                    d.""IntegrationId"", 
                    d.""RegisterDate"", 
                    d.""Sequence"", 
                    d.""TenantId"", 
                    d.""UpdateMode"", 
                    d.""Status"", 
                    d.""UniqueId""
                    FROM ""reporting-service"".deposits AS d
                    {access}
                    WHERE d.""RegistrationId"" < @FromId
                    {tenantWhere}
                    {where}
                    ORDER BY d.""RegistrationId"" {sorting}
                    LIMIT @Limit";
            }

            var limit = request.Take <= 0 ? 1000 : request.Take;

            try
            {
                var array = await context.Database.GetDbConnection()
                    .QueryAsync<Deposit>(searchQuery, new
                    {
                        MasterAffiliateId = request.MasterAffiliateId ?? 0,
                        TenantId = request.TenantId ?? string.Empty,
                        FromId = request.Cursor ?? 0,
                        Limit = limit,
                        AffiliateId = request.AffiliateId ?? 0,
                        RegistrationId = request.RegistrationId ?? 0,
                    });

                var response = array
                    .Select(MapToGrpcInner)
                    .ToArray();

                return new DepositSearchResponse()
                {
                    Deposits = response
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error happened {@context}", request);

                return new DepositSearchResponse()
                {
                    Error = new Error()
                    {
                        Message = "Internal error happened",
                        Type = ErrorType.Unknown
                    }
                };
            }
        }

        private static MarketingBox.Reporting.Service.Grpc.Models.Deposits.Deposit MapToGrpcInner(Deposit deposit, int arg2)
        {
            return new MarketingBox.Reporting.Service.Grpc.Models.Deposits.Deposit()
            {
                LeadId = deposit.RegistrationId,
                Sequence = deposit.Sequence,
                AffiliateId = deposit.AffiliateId,
                CampaignId = deposit.CampaignId,
                IntegrationId = deposit.IntegrationId,
                CrmStatus = deposit.CrmStatus,
                BrandId = deposit.BrandId,
                ConversionDate = deposit.ConversionDate?.UtcDateTime,
                Country = deposit.Country,
                CreatedAt = deposit.CreatedAt.UtcDateTime,
                CustomerId = deposit.CustomerId,
                Email = deposit.Email,
                RegisterDate = deposit.RegisterDate.UtcDateTime,
                TenantId = deposit.TenantId,
                UpdateMode = deposit.UpdateMode.MapEnum<MarketingBox.Reporting.Service.Domain.Deposit.DepositUpdateMode>(),
                UniqueId = deposit.UniqueId,
                RegistrationId = deposit.RegistrationId,
                Status = deposit.Status
            };
        }
    }
}
