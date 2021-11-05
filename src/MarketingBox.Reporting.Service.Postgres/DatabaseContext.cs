using MarketingBox.Reporting.Service.Postgres.ReadModels.Deposits;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Leads;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarketingBox.Reporting.Service.Postgres
{
    public class DatabaseContext : DbContext
    {
        private static readonly JsonSerializerSettings JsonSerializingSettings =
            new() { NullValueHandling = NullValueHandling.Ignore };

        public const string Schema = "reporting-service";

        public const string RegistrationTableName = "registrations";
        public const string ReportTableName = "reports";
        public const string DepositTableName = "deposits";

        public DbSet<Registration> Registrations { get; set; }

        public DbSet<ReportEntity> Reports { get; set; }

        public DbSet<Deposit> Deposits { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        public static ILoggerFactory LoggerFactory { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (LoggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(LoggerFactory).EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            SetLeadReadModel(modelBuilder);
            SetDepositReadModel(modelBuilder);
            SetReportEntity(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetLeadReadModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>().ToTable(RegistrationTableName);
            modelBuilder.Entity<Registration>().HasKey(e => e.RegistrationId);
            modelBuilder.Entity<Registration>().HasIndex(e => new { e.TenantId, e.RegistrationId });
            modelBuilder.Entity<Registration>().HasIndex(e => new { e.AffiliateId });
            modelBuilder.Entity<Registration>().Property(m => m.RegistrationId)
                .ValueGeneratedNever();
        }

        private void SetDepositReadModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Deposit>().ToTable(DepositTableName);
            modelBuilder.Entity<Deposit>().HasKey(e => new { e.RegistrationId, e.AffiliateId });
            modelBuilder.Entity<Deposit>().HasIndex(e => new { e.TenantId, e.RegistrationId });
            modelBuilder.Entity<Deposit>().HasIndex(e => new { e.AffiliateId });
        }

        private void SetReportEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportEntity>().ToTable(ReportTableName);
            modelBuilder.Entity<ReportEntity>().HasKey(x => new { x.AffiliateId, x.RegistrationId, x.ReportType });
            modelBuilder.Entity<ReportEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<ReportEntity>().HasIndex(x => x.TenantId);
            //modelBuilder.Entity<ReportEntity>().HasIndex(x => x.re);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
