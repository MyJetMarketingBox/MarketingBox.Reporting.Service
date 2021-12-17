using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class SearchAffiliateRegistrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer",
                schema: "reporting-service");

            migrationBuilder.CreateTable(
                name: "registrations_details",
                schema: "reporting-service",
                columns: table => new
                {
                    RegistrationUid = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    FirstName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    LastName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Phone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Ip = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Country = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    AffiliateId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    AffiliateName = table.Column<string>(type: "text", nullable: true),
                    ConversionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Sequence = table.Column<long>(type: "bigint", nullable: false),
                    CrmStatus = table.Column<int>(type: "integer", nullable: false),
                    RegistrationId = table.Column<long>(type: "bigint", nullable: false),
                    Integration = table.Column<string>(type: "text", nullable: true),
                    IntegrationId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ApprovedType = table.Column<int>(type: "integer", nullable: false),
                    Funnel = table.Column<string>(type: "text", nullable: true),
                    AffCode = table.Column<string>(type: "text", nullable: true),
                    Sub1 = table.Column<string>(type: "text", nullable: true),
                    Sub2 = table.Column<string>(type: "text", nullable: true),
                    Sub3 = table.Column<string>(type: "text", nullable: true),
                    Sub4 = table.Column<string>(type: "text", nullable: true),
                    Sub5 = table.Column<string>(type: "text", nullable: true),
                    Sub6 = table.Column<string>(type: "text", nullable: true),
                    Sub7 = table.Column<string>(type: "text", nullable: true),
                    Sub8 = table.Column<string>(type: "text", nullable: true),
                    Sub9 = table.Column<string>(type: "text", nullable: true),
                    Sub10 = table.Column<string>(type: "text", nullable: true),
                    CustomerId = table.Column<string>(type: "text", nullable: true),
                    CustomerToken = table.Column<string>(type: "text", nullable: true),
                    CustomerLoginUrl = table.Column<string>(type: "text", nullable: true),
                    CustomerBrand = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registrations_details", x => x.RegistrationUid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_AffiliateId",
                schema: "reporting-service",
                table: "registrations_details",
                column: "AffiliateId");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_ApprovedType",
                schema: "reporting-service",
                table: "registrations_details",
                column: "ApprovedType");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_Country",
                schema: "reporting-service",
                table: "registrations_details",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_CreatedAt",
                schema: "reporting-service",
                table: "registrations_details",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_Email",
                schema: "reporting-service",
                table: "registrations_details",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_RegistrationUid",
                schema: "reporting-service",
                table: "registrations_details",
                column: "RegistrationUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_TenantId",
                schema: "reporting-service",
                table: "registrations_details",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "registrations_details",
                schema: "reporting-service");

            migrationBuilder.CreateTable(
                name: "customer",
                schema: "reporting-service",
                columns: table => new
                {
                    UId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    AffiliateId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    Country = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CrmStatus = table.Column<int>(type: "integer", nullable: false),
                    DepositDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    FirstName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Ip = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    IsDeposit = table.Column<bool>(type: "boolean", nullable: false),
                    LastName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Phone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Sequence = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.UId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customer_Country",
                schema: "reporting-service",
                table: "customer",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_customer_CreatedDate",
                schema: "reporting-service",
                table: "customer",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_customer_Email",
                schema: "reporting-service",
                table: "customer",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_customer_IsDeposit",
                schema: "reporting-service",
                table: "customer",
                column: "IsDeposit");

            migrationBuilder.CreateIndex(
                name: "IX_customer_TenantId",
                schema: "reporting-service",
                table: "customer",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_customer_UId",
                schema: "reporting-service",
                table: "customer",
                column: "UId",
                unique: true);
        }
    }
}
