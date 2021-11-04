using System;
using System.Linq;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Grpc.Models.Partners;
using MarketingBox.Reporting.Service.Grpc.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Models.Reports.Requests;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.EntityFrameworkCore;
using Dapper;
using MarketingBox.Reporting.Service.Grpc.Models.Common;

namespace MarketingBox.Reporting.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

        public ReportService(ILogger<ReportService> logger,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
        {
            _logger = logger;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        public async Task<ReportSearchResponse> SearchAsync(ReportSearchRequest request)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

            var searchQuery = $@"
            CREATE TEMP TABLE reports_total (
            ""AffiliateId"" bigint NOT NULL,
            ""LeadId"" bigint NOT NULL,
            ""ReportType"" integer NOT NULL,
            ""TenantId"" text COLLATE pg_catalog.""default"",
            ""UniqueId"" text COLLATE pg_catalog.""default"",
            ""BoxId"" bigint NOT NULL,
            ""CampaignId"" bigint NOT NULL,
            ""BrandId"" bigint NOT NULL,
            ""CreatedAt"" timestamp with time zone NOT NULL,
            ""Payout"" numeric NOT NULL,
            ""Revenue"" numeric NOT NULL,
                CONSTRAINT ""PK_reports_total"" PRIMARY KEY(""AffiliateId"", ""LeadId"", ""ReportType"")
                ) ON COMMIT DROP;

            CREATE INDEX ""IX_reports_total_ReportType""
            ON reports_total USING btree
            (""ReportType"" ASC NULLS LAST)
            TABLESPACE pg_default;

            INSERT INTO reports_total
            SELECT* FROM ""reporting-service"".reports as rep
            where rep.""AffiliateId"" > @FromId and
            rep.""TenantId"" = @TenantId and
            rep.""CreatedAt"" >= @FromDate and
            rep.""CreatedAt"" <= @ToDate;

            select aggregateRep.""AffiliateId"", 
            SUM(aggregateRep.""SumPayout"") as ""SumPayout"", 
            SUM(aggregateRep.""SumRevenue"") as ""SumRevenue"", 
            Sum(aggregateRep.""LeadCount"") as ""LeadCount"", 
            Sum(aggregateRep.""DepositCount"") as ""DepositCount""
            from
                (SELECT rep.""AffiliateId"", SUM(""Payout"") as ""SumPayout"", SUM(""Revenue"") as ""SumRevenue"", COUNT(*) as ""LeadCount"", 0 As ""DepositCount""

            FROM reports_total as rep

            where rep.""ReportType"" = 0
            GROUP BY rep.""AffiliateId""
            UNION
                SELECT rep2.""AffiliateId"", SUM(""Payout"") as ""SumPayout"", SUM(""Revenue"") as ""SumRevenue"", 0 as ""LeadCount"", COUNT(*) As ""DepositCount""

            FROM reports_total as rep2

            where rep2.""ReportType"" = 1
            GROUP BY rep2.""AffiliateId"") as aggregateRep
            GROUP BY ""AffiliateId""
            ORDER BY ""AffiliateId""
            LIMIT @Limit;";

            try
            {
                var aggregatedReport = await context.Database.GetDbConnection()
                    .QueryAsync<AggregatedReportEntity>(searchQuery, new
                    {
                        TenantId = request.TenantId,
                        FromId = request.Cursor ?? 0,
                        FromDate = DateTime.SpecifyKind(request.FromDate, DateTimeKind.Utc),
                        ToDate = DateTime.SpecifyKind(request.ToDate, DateTimeKind.Utc),
                        Limit = request.Take,
                    });

                return new ReportSearchResponse()
                {
                    Reports = aggregatedReport.Select(x => new Report()
                    {
                        AffiliateId = x.AffiliateId,
                        Revenue = x.SumRevenue,
                        Payout = x.SumPayout,
                        Ctr = x.DepositCount / (decimal)x.LeadCount,
                        FtdCount = x.DepositCount,
                        LeadCount = x.LeadCount
                    }).ToArray()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error happened {@context}", request);

                return new ReportSearchResponse()
                {
                    Error = new Error()
                    {
                        Message = "Internal error happened",
                        Type = ErrorType.Unknown
                    }
                };
            }
        }

        public async Task<ReportByDaySearchResponse> SearchByDayAsync(ReportByDaySearchRequest request)
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

            var searchQuery = $@"
            CREATE TEMP TABLE reports_total (
            ""AffiliateId"" bigint NOT NULL,
            ""LeadId"" bigint NOT NULL,
            ""ReportType"" integer NOT NULL,
            ""TenantId"" text COLLATE pg_catalog.""default"",
            ""UniqueId"" text COLLATE pg_catalog.""default"",
            ""BoxId"" bigint NOT NULL,
            ""CampaignId"" bigint NOT NULL,
            ""BrandId"" bigint NOT NULL,
            ""CreatedAt"" timestamp with time zone NOT NULL,
            ""Payout"" numeric NOT NULL,
            ""Revenue"" numeric NOT NULL,
                CONSTRAINT ""PK_reports_total"" PRIMARY KEY(""AffiliateId"", ""LeadId"", ""ReportType"")
                ) ON COMMIT DROP;

            CREATE INDEX ""IX_reports_total_ReportType""
            ON reports_total USING btree
            (""ReportType"" ASC NULLS LAST)
            TABLESPACE pg_default;

            INSERT INTO reports_total
            SELECT* FROM ""reporting-service"".reports as rep
            where rep.""TenantId"" = @TenantId and
            rep.""CreatedAt"" >= @FromDate and
            rep.""CreatedAt"" <= @ToDate;;

            select date_trunc('day', aggregateRep.""CreatedAt"") as ""CreatedAt"", 
            Sum(aggregateRep.""LeadCount"") as ""LeadCount"", 
            Sum(aggregateRep.""DepositCount"") as ""DepositCount""
            from
                (SELECT date_trunc('day', rep.""CreatedAt"") as ""CreatedAt"", COUNT(*) as ""LeadCount"", 0 As ""DepositCount""

            FROM reports_total as rep

            where rep.""ReportType"" = 0
            GROUP BY date_trunc('day', rep.""CreatedAt"")
            UNION
                SELECT date_trunc('day', rep2.""CreatedAt"") as ""CreatedAt"", 0 as ""LeadCount"", COUNT(*) As ""DepositCount""

            FROM reports_total as rep2

            where rep2.""ReportType"" = 1
            GROUP BY date_trunc('day', rep2.""CreatedAt"")) as aggregateRep
            GROUP BY date_trunc('day', aggregateRep.""CreatedAt"")
            ORDER BY date_trunc('day', aggregateRep.""CreatedAt"")
            LIMIT @Limit;";

            try
            {
                var aggregatedReport = await context.Database.GetDbConnection()
                    .QueryAsync<AggregatedReportByDayEntity>(searchQuery, new
                    {
                        TenantId = request.TenantId,
                        FromId = request.Cursor ?? 0,
                        FromDate = DateTime.SpecifyKind(request.FromDate, DateTimeKind.Utc),
                        ToDate = DateTime.SpecifyKind(request.ToDate, DateTimeKind.Utc),
                        Limit = request.Take,
                    });

                return new ReportByDaySearchResponse()
                {
                    Reports = aggregatedReport.Select(x => new ReportByDay()
                    {
                        FtdCount = x.DepositCount,
                        LeadCount = x.LeadCount,
                        CreatedAt = x.CreatedAt.UtcDateTime
                    }).ToArray()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error happened {@context}", request);

                return new ReportByDaySearchResponse()
                {
                    Error = new Error()
                    {
                        Message = "Internal error happened",
                        Type = ErrorType.Unknown
                    }
                };
            }
        }
    }

    public class AggregatedReportEntity
    {
        public long AffiliateId { get; set; }
        public long SumPayout { get; set; }
        public long SumRevenue { get; set; }
        public long LeadCount { get; set; }
        public long DepositCount { get; set; }
    }

    public class AggregatedReportByDayEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public long LeadCount { get; set; }
        public long DepositCount { get; set; }
    }
}
