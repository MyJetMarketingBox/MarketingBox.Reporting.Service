using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models.Registrations;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Requests.Reports;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Reporting.Service.Repositories.Interfaces;
using MarketingBox.Reporting.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MarketingBox.Reporting.Service.Repositories;

public class RegistrationDetailsRepository : IRegistrationDetailsRepository
{
    private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
    private readonly ILogger<RegistrationDetailsRepository> _logger;
    private readonly IBrandBoxReportService _brandBoxReportService;

    public RegistrationDetailsRepository(
        DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder,
        ILogger<RegistrationDetailsRepository> logger,
        IBrandBoxReportService brandBoxReportService)
    {
        _dbContextOptionsBuilder = dbContextOptionsBuilder;
        _logger = logger;
        _brandBoxReportService = brandBoxReportService;
    }

    public async Task SaveAsync(RegistrationDetails entity)
    {
        await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

        await context.RegistrationDetails
            .Upsert(entity)
            .RunAsync();
        await context.SaveChangesAsync();
    }

    public async Task<(List<Report>, int)> SearchAsync(ReportSearchRequest request)
    {
        try
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

            _logger.LogInformation("Getting script {ScriptName}", $"ReportBy{request.ReportType}.sql");

            var scriptName = $"ReportBy{request.ReportType}.sql";
            var path = Path.Combine(Environment.CurrentDirectory, @"Scripts/", scriptName);
            using var script = new StreamReader(path);
            var scriptBody = await script.ReadToEndAsync();

            await using var command = context.Database.GetDbConnection().CreateCommand();

            var brandIds = new List<long>();
            if (request.BrandBoxIds != null && request.BrandBoxIds.Any())
            {
                var parameters = new List<string>();
                brandIds = await _brandBoxReportService.GetBrandIdsFromBrandBoxes(request.BrandBoxIds);
                for (int i = 0; i < brandIds.Count; i++)
                {
                    var brandIdsParameter = command.CreateParameter();
                    brandIdsParameter.ParameterName = $"@BrandId{i}";
                    parameters.Add(brandIdsParameter.ParameterName);
                    brandIdsParameter.Value = brandIds[i];
                    command.Parameters.Add(brandIdsParameter);
                }

                scriptBody = scriptBody
                    .Replace("@BrandIds", string.Join(',', parameters));
            }
            else
            {
                scriptBody = scriptBody
                    .Replace("and rd.\"BrandId\" in (@BrandIds)", "");
            }

            var selectReport = @"select * from report";
            var paginatedQuery = new StringBuilder();
            paginatedQuery.AppendLine(selectReport);
            if (request.Asc)
            {
                if (request.Cursor.HasValue)
                    paginatedQuery.AppendLine($"where id > {request.Cursor}");
                paginatedQuery.AppendLine("order by id");
            }
            else
            {
                if (request.Cursor.HasValue)
                    paginatedQuery.AppendLine($"where id < {request.Cursor}");
                paginatedQuery.AppendLine("order by id desc");
            }

            if (request.Take.HasValue)
            {
                paginatedQuery.AppendLine($"limit {request.Take}");
            }

            scriptBody = scriptBody.Replace(selectReport, paginatedQuery.ToString());

            command.CommandText = scriptBody;
            command.CommandType = CommandType.Text;

            _logger.LogInformation("Executing script {Script}", scriptBody);

            GenerateParametersForFilter(request, command);
            await context.Database.OpenConnectionAsync();
            await using var result = await command.ExecuteReaderAsync();

            _logger.LogInformation("Script was executed");
            var total = 0;

            var entities = new List<Report>();
            while (await result.ReadAsync())
            {
                total = result.GetInt32(0);

                if (!await result.NextResultAsync()) continue;
                while (await result.ReadAsync())
                {
                    entities.Add(new Report
                    {
                        Id = result.GetInt64(0),
                        Name = SafeGet<string>(result, 1),
                        RegistrationCount = result.GetInt32(2),
                        FtdCount = result.GetInt32(3),
                        FailedCount = result.GetInt32(4),
                        UnassignedCount = result.GetInt32(5),
                        // Revenue = result.GetDecimal(6),
                        // Payout = result.GetDecimal(7),
                        // Epc = SafeGet<decimal?>(result, 8),
                        // Clicks = SafeGet<decimal?>(result, 9),
                        // Pl = SafeGet<decimal>(result, 10),
                        // Cr = SafeGet<decimal?>(result, 11),
                        // Epl = SafeGet<decimal?>(result, 12),
                        // Roi = SafeGet<decimal?>(result, 13)
                    });
                }
            }

            await context.Database.CloseConnectionAsync();

            _logger.LogInformation("{Count} rows were read", entities.Count);

            return (entities, total);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    private static T SafeGet<T>(IDataRecord reader, int colIndex)
    {
        return reader.IsDBNull(colIndex) ? default : (T) reader.GetValue(colIndex);
    }

    private static void GenerateParametersForFilter(ReportSearchRequest requestFilter, DbCommand command)
    {
        var affiliateId = command.CreateParameter();
        affiliateId.ParameterName = "@AffiliateId";
        affiliateId.DbType = DbType.Int64;
        affiliateId.Value = (object) requestFilter.AffiliateId ?? DBNull.Value;
        command.Parameters.Add(affiliateId);

        var tenantId = command.CreateParameter();
        tenantId.ParameterName = "@TenantId";
        tenantId.DbType = DbType.String;
        tenantId.Value = !string.IsNullOrEmpty(requestFilter.TenantId) ? requestFilter.TenantId : DBNull.Value;
        command.Parameters.Add(tenantId);

        var country = command.CreateParameter();
        country.ParameterName = "@Country";
        country.DbType = DbType.String;
        country.Value = !string.IsNullOrEmpty(requestFilter.CountryCode) ? requestFilter.CountryCode : DBNull.Value;
        command.Parameters.Add(country);

        var brandId = command.CreateParameter();
        brandId.ParameterName = "@BrandId";
        brandId.DbType = DbType.Int64;
        brandId.Value = (object) requestFilter.BrandId ?? DBNull.Value;
        command.Parameters.Add(brandId);

        var fromDate = command.CreateParameter();
        fromDate.ParameterName = "@FromDate";
        fromDate.DbType = DbType.DateTimeOffset;
        fromDate.Value =
            requestFilter.FromDate.HasValue
                ? DateTime.SpecifyKind(requestFilter.FromDate.Value, DateTimeKind.Utc)
                : DBNull.Value;
        command.Parameters.Add(fromDate);

        var toDate = command.CreateParameter();
        toDate.ParameterName = "@ToDate";
        toDate.DbType = DbType.DateTimeOffset;
        toDate.Value =
            requestFilter.ToDate.HasValue
                ? DateTime.SpecifyKind(requestFilter.ToDate.Value.Add(new TimeSpan(23, 59, 59)),
                    DateTimeKind.Utc)
                : DBNull.Value;
        command.Parameters.Add(toDate);
    }
}