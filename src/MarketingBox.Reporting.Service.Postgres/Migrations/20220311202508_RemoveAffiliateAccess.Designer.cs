﻿// <auto-generated />
using System;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220311202508_RemoveAffiliateAccess")]
    partial class RemoveAffiliateAccess
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("reporting-service")
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MarketingBox.Reporting.Service.Domain.Models.RegistrationDetails", b =>
                {
                    b.Property<string>("RegistrationUid")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

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

                    b.Property<string>("Country")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

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
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Funnel")
                        .HasColumnType("text");

                    b.Property<string>("Integration")
                        .HasColumnType("text");

                    b.Property<long>("IntegrationId")
                        .HasColumnType("bigint");

                    b.Property<string>("Ip")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("LastName")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Phone")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

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
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("UpdateMode")
                        .HasColumnType("integer");

                    b.HasKey("RegistrationUid");

                    b.HasIndex("AffiliateId");

                    b.HasIndex("Country");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("Email");

                    b.HasIndex("RegistrationUid")
                        .IsUnique();

                    b.HasIndex("TenantId");

                    b.HasIndex("UpdateMode");

                    b.ToTable("registrations_details", "reporting-service");
                });

            modelBuilder.Entity("MarketingBox.Reporting.Service.Domain.Models.Reports.BrandEntity", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("TenantId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("PayoutAmount")
                        .HasColumnType("numeric");

                    b.Property<int>("PayoutCurrency")
                        .HasColumnType("integer");

                    b.Property<int>("PayoutPlan")
                        .HasColumnType("integer");

                    b.Property<decimal>("RevenueAmount")
                        .HasColumnType("numeric");

                    b.Property<int>("RevenueCurrency")
                        .HasColumnType("integer");

                    b.Property<int>("RevenuePlan")
                        .HasColumnType("integer");

                    b.HasKey("Id", "TenantId");

                    b.ToTable("brands", "reporting-service");
                });
#pragma warning restore 612, 618
        }
    }
}
