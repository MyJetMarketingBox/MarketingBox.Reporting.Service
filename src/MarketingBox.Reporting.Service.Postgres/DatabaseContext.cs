using MarketingBox.Reporting.Service.Domain.Models.Registrations;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Domain.Models.TrackingLinks;
using Microsoft.EntityFrameworkCore;
using MyJetWallet.Sdk.Postgres;

namespace MarketingBox.Reporting.Service.Postgres
{
    public class DatabaseContext : MyDbContext
    {
        public const string Schema = "reporting-service";

        private const string RegistrationDetailsTableName = "registrations_details";
        private const string TrackingLinkTable = "trackinglinks";
        private const string BrandsTableName = "brands";
        public DbSet<RegistrationDetails> RegistrationDetails { get; set; }
        public DbSet<BrandEntity> Brands { get; set; }
        public DbSet<TrackingLink> TrackingLinks { get; set; }

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

            SetRegistrationDetailsModel(modelBuilder);
            SetBrandModel(modelBuilder);
            SetTrackingLinkModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetBrandModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BrandEntity>().ToTable(BrandsTableName);
            modelBuilder.Entity<BrandEntity>().HasKey(x => new {x.Id, x.TenantId});
        }

        private static void SetRegistrationDetailsModel(ModelBuilder modelBuilder)
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

        private static void SetTrackingLinkModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrackingLink>().ToTable(TrackingLinkTable);
            modelBuilder.Entity<TrackingLink>().OwnsOne(x => x.LinkParameterValues);
            modelBuilder.Entity<TrackingLink>().OwnsOne(x => x.LinkParameterNames);
            
            modelBuilder.Entity<TrackingLink>().Property(x => x.Id).IsRequired();
            modelBuilder.Entity<TrackingLink>().Property(x => x.Link).IsRequired();
            modelBuilder.Entity<TrackingLink>().Property(x => x.BrandId).IsRequired();
            modelBuilder.Entity<TrackingLink>().Property(x => x.AffiliateId).IsRequired();
            modelBuilder.Entity<TrackingLink>().Property(x => x.UniqueId).IsRequired();
            modelBuilder.Entity<TrackingLink>().Property(x => x.ClickId).IsRequired();

            modelBuilder.Entity<TrackingLink>().HasKey(x => new {x.Id, x.ClickId});
            modelBuilder.Entity<TrackingLink>().HasIndex(x => x.ClickId).IsUnique();
            modelBuilder.Entity<TrackingLink>().HasIndex(x => x.UniqueId);
        }

    }
}