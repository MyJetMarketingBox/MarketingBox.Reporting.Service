﻿// <auto-generated />
using System;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("reporting-service")
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MarketingBox.Reporting.Service.Domain.Models.Brands.BrandEntity", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("TenantId")
                        .HasColumnType("text");

                    b.Property<long?>("IntegrationId")
                        .HasColumnType("bigint");

                    b.Property<int>("IntegrationType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id", "TenantId");

                    b.ToTable("brands", "reporting-service");
                });

            modelBuilder.Entity("MarketingBox.Reporting.Service.Domain.Models.Registrations.RegistrationDetails", b =>
                {
                    b.Property<string>("RegistrationUid")
                        .HasColumnType("text");

                    b.Property<string>("AffCode")
                        .HasColumnType("text");

                    b.Property<long>("AffiliateId")
                        .HasColumnType("bigint");

                    b.Property<string>("AffiliateName")
                        .HasColumnType("text");

                    b.Property<bool>("AutologinUsed")
                        .HasColumnType("boolean");

                    b.Property<long>("BrandId")
                        .HasColumnType("bigint");

                    b.Property<long>("CampaignId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ConversionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CountryAlfa2Code")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CrmStatus")
                        .HasColumnType("integer");

                    b.Property<string>("CustomerBrand")
                        .HasColumnType("text");

                    b.Property<string>("CustomerId")
                        .HasColumnType("text");

                    b.Property<string>("CustomerLoginUrl")
                        .HasColumnType("text");

                    b.Property<string>("CustomerToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DepositDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("Funnel")
                        .HasColumnType("text");

                    b.Property<string>("Integration")
                        .HasColumnType("text");

                    b.Property<long>("IntegrationId")
                        .HasColumnType("bigint");

                    b.Property<string>("Ip")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<long>("RegistrationId")
                        .HasColumnType("bigint");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Sub1")
                        .HasColumnType("text");

                    b.Property<string>("Sub10")
                        .HasColumnType("text");

                    b.Property<string>("Sub2")
                        .HasColumnType("text");

                    b.Property<string>("Sub3")
                        .HasColumnType("text");

                    b.Property<string>("Sub4")
                        .HasColumnType("text");

                    b.Property<string>("Sub5")
                        .HasColumnType("text");

                    b.Property<string>("Sub6")
                        .HasColumnType("text");

                    b.Property<string>("Sub7")
                        .HasColumnType("text");

                    b.Property<string>("Sub8")
                        .HasColumnType("text");

                    b.Property<string>("Sub9")
                        .HasColumnType("text");

                    b.Property<string>("TenantId")
                        .HasColumnType("text");

                    b.Property<int>("UpdateMode")
                        .HasColumnType("integer");

                    b.HasKey("RegistrationUid");

                    b.HasIndex("AffiliateId");

                    b.HasIndex("CountryAlfa2Code");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("Email");

                    b.HasIndex("RegistrationUid")
                        .IsUnique();

                    b.HasIndex("TenantId");

                    b.HasIndex("UpdateMode");

                    b.ToTable("registrations_details", "reporting-service");
                });

            modelBuilder.Entity("MarketingBox.Reporting.Service.Domain.Models.TrackingLinks.TrackingLink", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<long>("ClickId")
                        .HasColumnType("bigint");

                    b.Property<long>("AffiliateId")
                        .HasColumnType("bigint");

                    b.Property<long>("BrandId")
                        .HasColumnType("bigint");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("RegistrationId")
                        .HasColumnType("bigint");

                    b.Property<string>("UniqueId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id", "ClickId");

                    b.HasIndex("ClickId")
                        .IsUnique();

                    b.HasIndex("UniqueId");

                    b.ToTable("trackinglinks", "reporting-service");
                });

            modelBuilder.Entity("MarketingBox.Reporting.Service.Domain.Models.TrackingLinks.TrackingLink", b =>
                {
                    b.OwnsOne("MarketingBox.Reporting.Service.Domain.Models.TrackingLinks.LinkParameterNames", "LinkParameterNames", b1 =>
                        {
                            b1.Property<long>("TrackingLinkId")
                                .HasColumnType("bigint");

                            b1.Property<long>("TrackingLinkClickId")
                                .HasColumnType("bigint");

                            b1.Property<string>("ClickId")
                                .HasColumnType("text");

                            b1.Property<string>("Language")
                                .HasColumnType("text");

                            b1.Property<string>("MPC_1")
                                .HasColumnType("text");

                            b1.Property<string>("MPC_2")
                                .HasColumnType("text");

                            b1.Property<string>("MPC_3")
                                .HasColumnType("text");

                            b1.Property<string>("MPC_4")
                                .HasColumnType("text");

                            b1.HasKey("TrackingLinkId", "TrackingLinkClickId");

                            b1.ToTable("trackinglinks", "reporting-service");

                            b1.WithOwner()
                                .HasForeignKey("TrackingLinkId", "TrackingLinkClickId");
                        });

                    b.OwnsOne("MarketingBox.Reporting.Service.Domain.Models.TrackingLinks.LinkParameterValues", "LinkParameterValues", b1 =>
                        {
                            b1.Property<long>("TrackingLinkId")
                                .HasColumnType("bigint");

                            b1.Property<long>("TrackingLinkClickId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Language")
                                .HasColumnType("text");

                            b1.Property<string>("MPC_1")
                                .HasColumnType("text");

                            b1.Property<string>("MPC_2")
                                .HasColumnType("text");

                            b1.Property<string>("MPC_3")
                                .HasColumnType("text");

                            b1.Property<string>("MPC_4")
                                .HasColumnType("text");

                            b1.HasKey("TrackingLinkId", "TrackingLinkClickId");

                            b1.ToTable("trackinglinks", "reporting-service");

                            b1.WithOwner()
                                .HasForeignKey("TrackingLinkId", "TrackingLinkClickId");
                        });

                    b.Navigation("LinkParameterNames");

                    b.Navigation("LinkParameterValues");
                });
#pragma warning restore 612, 618
        }
    }
}
