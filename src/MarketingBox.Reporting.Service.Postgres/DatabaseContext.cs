using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using Microsoft.EntityFrameworkCore;
using MyJetWallet.Sdk.Postgres;

namespace MarketingBox.Reporting.Service.Postgres
{
    public class DatabaseContext : MyDbContext
    {
        public const string Schema = "reporting-service";

        private const string AffiliateAccessTableName = "affiliate_access";
        private const string RegistrationDetailsTableName = "registrations_details";
        private const string BrandsTableName = "brands";
        public DbSet<AffiliateAccess> AffiliateAccesses { get; set; }
        public DbSet<RegistrationDetails> RegistrationDetails { get; set; }
        public DbSet<BrandEntity> Brands { get; set; }

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
            SetRegistrationDetailsModel(modelBuilder);
            SetBrandModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetBrandModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BrandEntity>().ToTable(BrandsTableName);
            modelBuilder.Entity<BrandEntity>().HasKey(x => new {x.Id, x.TenantId});
            modelBuilder.Entity<BrandEntity>().OwnsOne(x => x.Payout);
            modelBuilder.Entity<BrandEntity>().OwnsOne(x => x.Revenue);
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
            modelBuilder.Entity<AffiliateAccess>().HasIndex(e => new {e.MasterAffiliateId, e.AffiliateId}).IsUnique();
            modelBuilder.Entity<AffiliateAccess>().HasIndex(e => e.AffiliateId);

            modelBuilder.Entity<AffiliateAccess>().Property(m => m.Id)
                .ValueGeneratedNever();
        }
    }
}