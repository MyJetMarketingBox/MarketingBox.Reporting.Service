using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Postgres.ReadModels.AffiliateAccesses;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Deposits;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Registrations;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Reports;
using Microsoft.EntityFrameworkCore;
using MyJetWallet.Sdk.Postgres;

namespace MarketingBox.Reporting.Service.Postgres
{
    public class DatabaseContext : MyDbContext
    {
        public const string Schema = "reporting-service";

        private const string RegistrationTableName = "registrations";
        private const string ReportTableName = "reports";
        private const string DepositTableName = "deposits";
        private const string AffiliateAccessTableName = "affiliate_access";
        private const string RegistrationDetailsTableName = "registrations_details";

        public DbSet<Registration> Registrations { get; set; }
        public DbSet<ReportEntity> Reports { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<AffiliateAccess> AffiliateAccesses { get; set; }
        public DbSet<RegistrationDetails> RegistrationDetails { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

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

            SetAffiliateAccessReadModel(modelBuilder);
            SetRegistrationReadModel(modelBuilder);
            SetDepositReadModel(modelBuilder);
            SetReportEntity(modelBuilder);
            SetRegistrationDetailsModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetRegistrationDetailsModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegistrationDetails>().ToTable(RegistrationDetailsTableName);
            
            modelBuilder.Entity<RegistrationDetails>().HasKey(e => e.RegistrationUid);
            
            modelBuilder.Entity<RegistrationDetails>().Property(e => e.RegistrationUid).HasMaxLength(64);
            modelBuilder.Entity<RegistrationDetails>().Property(e => e.TenantId).HasMaxLength(64);
            modelBuilder.Entity<RegistrationDetails>().Property(e => e.FirstName).HasMaxLength(64);
            modelBuilder.Entity<RegistrationDetails>().Property(e => e.LastName).HasMaxLength(64);
            modelBuilder.Entity<RegistrationDetails>().Property(e => e.Email).HasMaxLength(128);
            modelBuilder.Entity<RegistrationDetails>().Property(e => e.Phone).HasMaxLength(64);
            modelBuilder.Entity<RegistrationDetails>().Property(e => e.Ip).HasMaxLength(64);
            modelBuilder.Entity<RegistrationDetails>().Property(e => e.Country).HasMaxLength(64);
            
            modelBuilder.Entity<RegistrationDetails>().HasIndex(e => e.RegistrationUid).IsUnique();
            modelBuilder.Entity<RegistrationDetails>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<RegistrationDetails>().HasIndex(e => e.AffiliateId);
            modelBuilder.Entity<RegistrationDetails>().HasIndex(e => e.Email);
            modelBuilder.Entity<RegistrationDetails>().HasIndex(e => e.Country);
            modelBuilder.Entity<RegistrationDetails>().HasIndex(e => e.CreatedAt);
            modelBuilder.Entity<RegistrationDetails>().HasIndex(e => e.UpdateMode);
        }

        private void SetAffiliateAccessReadModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AffiliateAccess>().ToTable(AffiliateAccessTableName);
            modelBuilder.Entity<AffiliateAccess>().HasKey(x => x.Id);
            modelBuilder.Entity<AffiliateAccess>().HasIndex(e => new { e.MasterAffiliateId, e.AffiliateId }).IsUnique();
            modelBuilder.Entity<AffiliateAccess>().HasIndex(e => e.AffiliateId);

            modelBuilder.Entity<AffiliateAccess>().Property(m => m.Id)
                .ValueGeneratedNever();

            //modelBuilder.Entity<AffiliateAccess>()
            //    .HasMany(x => x.Deposits)
            //    .WithMany(x => x.AffiliateAccesses)
            //    .(x => x.AffiliateId);

            //modelBuilder.Entity<AffiliateAccess>()
            //    .HasMany(x => x.Registration)
            //    .WithMany(x => x.AffiliateAccesses)
            //    .HasForeignKey(x => x.AffiliateId);
        }

        private void SetRegistrationReadModel(ModelBuilder modelBuilder)
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
            modelBuilder.Entity<Deposit>().HasKey(e => e.RegistrationId);
            modelBuilder.Entity<Deposit>().HasIndex(e => new { e.TenantId, e.RegistrationId });
            modelBuilder.Entity<Deposit>().HasIndex(e => new { e.AffiliateId });
            modelBuilder.Entity<Deposit>().Property(m => m.RegistrationId)
                .ValueGeneratedNever();

            //modelBuilder.Entity<Deposit>().HasMany(x => x.AffiliateAccesses).
        }

        private void SetReportEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportEntity>().ToTable(ReportTableName);
            modelBuilder.Entity<ReportEntity>().HasKey(x => new { x.AffiliateId, x.RegistrationId, x.ReportType });
            modelBuilder.Entity<ReportEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<ReportEntity>().HasIndex(x => x.TenantId);
        }
    }
}
