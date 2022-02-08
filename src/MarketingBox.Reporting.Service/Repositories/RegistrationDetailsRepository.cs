using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MarketingBox.Reporting.Service.Repositories;

public class RegistrationDetailsRepository : IRegistrationDetailsRepository
{
    private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
    private readonly ILogger<RegistrationDetailsRepository> _logger;

    // private static string SearchQuery(ReportSearchRequest request) => 
    //     $@"select @Type as Name,
    //               count(*) filter (where rd.""Status"" = {(int) RegistrationStatus.Registered}) as RegistrationCount,
    //               count(*) filter (where rd.""Status"" = {(int) RegistrationStatus.Approved}) as FtdCount
    //        from ""reporting-service"".registrations_details rd
    //        where {GenerateFilter(request)}
    //        group by @Type
    //        order by @Type
    //        limit @Limit";

    public RegistrationDetailsRepository(
        DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder,
        ILogger<RegistrationDetailsRepository> logger)
    {
        _dbContextOptionsBuilder = dbContextOptionsBuilder;
        _logger = logger;
    }

    public async Task SaveAsync(RegistrationDetails entity)
    {
        await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

        await context.RegistrationDetails
            .Upsert(entity)
            .RunAsync();
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AggregatedReportEntity>> SearchAsync(ReportSearchRequest request)
    {
        await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);


        var query = context.RegistrationDetails.AsQueryable();

        var filtered = GenerateFilter(query, request);

        var group = request.ReportType switch
        {
            ReportType.Affiliate => filtered.GroupBy(x => new KeyType{ Key = x.AffiliateId.ToString(), Brand = x.BrandId}),
            ReportType.Brand => filtered.GroupBy(x => new KeyType{ Key = x.BrandId.ToString() }),
            ReportType.Country => filtered.GroupBy(x => new KeyType{ Key = x.Country, Brand = x.BrandId}),
            ReportType.Day => filtered.GroupBy(x => new KeyType{ Key = x.CreatedAt.Day.ToString(), Brand = x.BrandId}),
            ReportType.Month => filtered.GroupBy(x => new KeyType{ Key = x.CreatedAt.Month.ToString(), Brand = x.BrandId}),
            ReportType.Offer => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException()
        };
        var grouped =  group
            .Select(x =>
                new AggregatedReportEntity
                {
                    Name = x.Key.Key,
                    Brand = x.Key.Brand,
                    FtdCount = x.Count(z => z.Status == RegistrationStatus.Approved),
                    RegistrationCount = x.Count(z => z.Status == RegistrationStatus.Registered)
                });

        var ordered = request.Asc
            ? grouped.OrderBy(x => x.Name)
            : grouped.OrderByDescending(x => x.Name);
        var trimmed = ordered.Take(request.Take);
        await trimmed.LoadAsync();

        var reportEntities = trimmed.ToList();

        // var type = request.ReportType switch
        // {
        //     ReportType.Affiliate => @"rd.""AffiliateId""",
        //     ReportType.Brand => @"rd.""IntegrationId""",
        //     ReportType.Country => @"rd.""Country""",
        //     ReportType.Day => @"rd.""CreatedAt""",
        //     ReportType.Month => @"rd.""CreatedAt""",
        //     ReportType.Offer => throw new NotImplementedException(),
        //     _ => throw new ArgumentOutOfRangeException()
        // };

        // var aggregatedReport =
        //     await context.Database
        //         .GetDbConnection()
        //         .QueryAsync<AggregatedReportEntity>(
        //             SearchQuery(request),
        //             new
        //             {
        //                 Type = type,
        //                 AffiliateId = request.AffiliateId,
        //                 Country = request.Country,
        //                 BrandId = request.BrandId,
        //                 Limit = request.Take,
        //                 FromDate = request.FromDate.HasValue
        //                     ? DateTime.SpecifyKind(request.FromDate.Value, DateTimeKind.Utc)
        //                     : default,
        //                 ToDate = request.ToDate.HasValue
        //                     ? DateTime.SpecifyKind(request.ToDate.Value.Add(new TimeSpan(23, 59, 59)), DateTimeKind.Utc)
        //                     : default,
        //                 //Order = request.Asc ? "asc" : "desc"
        //             });
        return reportEntities;
    }

    public async Task SearchByDateAsync()
    {
        await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
        await Task.CompletedTask;
    }

    private static IQueryable<RegistrationDetails> GenerateFilter(
        IQueryable<RegistrationDetails> filter,
        ReportSearchRequest requestFilter)
    {
        if (!string.IsNullOrEmpty(requestFilter.Country))
        {
            filter = filter.Where(x => x.Country == requestFilter.Country);
        }

        if (requestFilter.BrandId is not null)
        {
            filter = filter.Where(x => x.BrandId == requestFilter.BrandId);
        }

        if (requestFilter.Offer is not null)
        {
            throw new NotImplementedException();
        }

        if (requestFilter.AffiliateId is not null)
        {
            filter = filter.Where(x => x.AffiliateId == requestFilter.AffiliateId);
        }

        if (requestFilter.FromDate is not null)
        {
            filter = filter.Where(x =>
                x.CreatedAt.Date >= DateTime.SpecifyKind(requestFilter.FromDate.Value, DateTimeKind.Utc));
        }

        if (requestFilter.ToDate is not null)
        {
            filter = filter.Where(x =>
                x.CreatedAt.Date <= DateTime.SpecifyKind(requestFilter.ToDate.Value.Add(new TimeSpan(23, 59, 59)),
                    DateTimeKind.Utc));
        }

        return filter;
    }
}

public class AggregatedReportEntity
{
    public string Name { get; set; }
    public long RegistrationCount { get; set; }
    public long FtdCount { get; set; }

    public decimal Payout { get; set; }

    public decimal Revenue { get; set; }

    public decimal Cr { get; set; }
    public long? Brand { get; set; }
}

public class  KeyType
{
    public string Key { get; set; }
    public long? Brand { get; set; }
}

public class AggregatedReportByDayEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    public long RegistrationCount { get; set; }
    public long DepositCount { get; set; }
}

// private static string GenerateFilter(ReportSearchRequest requestFilter)
// {
//     var filter = new StringBuilder();
//     if (!string.IsNullOrEmpty(requestFilter.Country))
//     {
//         filter.Append(@"rd.""Country"" = @Country AND ");
//     }
//
//     if (requestFilter.BrandId is not null)
//     {
//         filter.Append(@"rd.""BrandId"" = @BrandId AND ");
//     }
//
//     if (requestFilter.Offer is not null)
//     {
//         filter.Append("1=1 AND ");
//     }
//
//     if (requestFilter.AffiliateId is not null)
//     {
//         filter.Append(@"rd.""AffiliateId"" = @AffiliateId AND ");
//     }
//
//     if (requestFilter.FromDate is not null)
//     {
//         filter.Append(@"rd.""FromDate"" >= @FromDate AND ");
//     }
//
//     if (requestFilter.ToDate is not null)
//     {
//         filter.Append(@"rd.""ToDate"" <= @ToDate AND ");
//     }
//
//     filter.Append("1=1");
//
//     return filter.ToString();
// }