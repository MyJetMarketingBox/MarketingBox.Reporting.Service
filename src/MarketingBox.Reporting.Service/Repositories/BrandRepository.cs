using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Reporting.Service.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MarketingBox.Reporting.Service.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly ILogger<BrandRepository> _logger;
    private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

    public BrandRepository(
        DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder,
        ILogger<BrandRepository> logger)
    {
        _dbContextOptionsBuilder = dbContextOptionsBuilder;
        _logger = logger;
    }
    
    
    public async Task CreateOrUpdateAsync(IEnumerable<BrandEntity> brandEntities)
    {
        try
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            await context.Brands.UpsertRange(brandEntities).RunAsync();
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task DeleteAsync(IEnumerable<BrandEntity> brandEntities)
    {
        try
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            await context.Brands.DeleteRangeByKeyAsync(brandEntities);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}