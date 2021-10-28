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
using MarketingBox.Reporting.Service.Domain.Extensions;

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

            try
            {
                var query = context.Deposits.AsQueryable();

                if (!string.IsNullOrEmpty(request.TenantId))
                {
                    query = query.Where(x => x.TenantId == request.TenantId);
                }

                if (request.AffiliateId.HasValue)
                {
                    query = query.Where(x => x.AffiliateId == request.AffiliateId);
                }

                if (request.LeadId.HasValue)
                {
                    query = query.Where(x => x.LeadId == request.LeadId);
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

        private MarketingBox.Reporting.Service.Grpc.Models.Deposits.Deposit MapToGrpcInner(Postgres.ReadModels.Deposits.Deposit deposit, int arg2)
        {
            return new MarketingBox.Reporting.Service.Grpc.Models.Deposits.Deposit()
            {
                LeadId = deposit.LeadId,
                Sequence = deposit.Sequence,
                AffiliateId = deposit.AffiliateId,
                BoxId = deposit.BoxId,
                BrandId = deposit.BrandId,
                BrandStatus = deposit.BrandStatus,
                CampaignId = deposit.CampaignId,
                ConversionDate = deposit.ConversionDate?.UtcDateTime,
                Country = deposit.Country,
                CreatedAt = deposit.CreatedAt.UtcDateTime,
                CustomerId = deposit.CustomerId,
                Email = deposit.Email,
                RegisterDate = deposit.RegisterDate.UtcDateTime,
                TenantId = deposit.TenantId,
                Type = deposit.Type.MapEnum<MarketingBox.Reporting.Service.Domain.Models.Deposit.ApprovedType>(),
                UniqueId = deposit.UniqueId,
            };
        }
    }
}
