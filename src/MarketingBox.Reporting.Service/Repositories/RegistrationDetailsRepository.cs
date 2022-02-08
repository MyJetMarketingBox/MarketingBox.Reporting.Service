using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MarketingBox.Reporting.Service.Repositories
{
    public interface IRegistrationDetailsRepository
    {
        Task SaveAsync(RegistrationDetails entity);
        Task SearchAsync();
        Task SearchByDateAsync();
    }

    public class RegistrationDetailsRepository : IRegistrationDetailsRepository
    {
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
        private readonly ILogger<RegistrationDetailsRepository> _logger;

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
        
        public async Task SearchAsync()
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            await Task.CompletedTask;
        }
        
        public async Task SearchByDateAsync()
        {
            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            await Task.CompletedTask;
        }
    }
}