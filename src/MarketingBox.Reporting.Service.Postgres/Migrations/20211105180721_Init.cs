using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "reporting-service");

            migrationBuilder.CreateTable(
                name: "deposits",
                schema: "reporting-service",
                columns: table => new
                {
                    RegistrationId = table.Column<long>(type: "bigint", nullable: false),
                    AffiliateId = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: true),
                    UniqueId = table.Column<string>(type: "text", nullable: true),
                    CustomerId = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    IntegrationId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    RegisterDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ConversionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Sequence = table.Column<long>(type: "bigint", nullable: false),
                    BrandStatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposits", x => new { x.RegistrationId, x.AffiliateId });
                });

            migrationBuilder.CreateTable(
                name: "registrations",
                schema: "reporting-service",
                columns: table => new
                {
                    RegistrationId = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: true),
                    UniqueId = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Ip = table.Column<string>(type: "text", nullable: true),
                    AffiliateId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    IntegrationId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CrmStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Sequence = table.Column<long>(type: "bigint", nullable: false),
                    So = table.Column<string>(type: "text", nullable: true),
                    Sub = table.Column<string>(type: "text", nullable: true),
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
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ConversionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DepositDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registrations", x => x.RegistrationId);
                });

            migrationBuilder.CreateTable(
                name: "reports",
                schema: "reporting-service",
                columns: table => new
                {
                    AffiliateId = table.Column<long>(type: "bigint", nullable: false),
                    RegistrationId = table.Column<long>(type: "bigint", nullable: false),
                    ReportType = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: true),
                    UniqueId = table.Column<string>(type: "text", nullable: true),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    IntegrationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Payout = table.Column<decimal>(type: "numeric", nullable: false),
                    Revenue = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => new { x.AffiliateId, x.RegistrationId, x.ReportType });
                });

            migrationBuilder.CreateIndex(
                name: "IX_deposits_AffiliateId",
                schema: "reporting-service",
                table: "deposits",
                column: "AffiliateId");

            migrationBuilder.CreateIndex(
                name: "IX_deposits_TenantId_RegistrationId",
                schema: "reporting-service",
                table: "deposits",
                columns: new[] { "TenantId", "RegistrationId" });

            migrationBuilder.CreateIndex(
                name: "IX_registrations_AffiliateId",
                schema: "reporting-service",
                table: "registrations",
                column: "AffiliateId");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_TenantId_RegistrationId",
                schema: "reporting-service",
                table: "registrations",
                columns: new[] { "TenantId", "RegistrationId" });

            migrationBuilder.CreateIndex(
                name: "IX_reports_CreatedAt",
                schema: "reporting-service",
                table: "reports",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_reports_TenantId",
                schema: "reporting-service",
                table: "reports",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deposits",
                schema: "reporting-service");

            migrationBuilder.DropTable(
                name: "registrations",
                schema: "reporting-service");

            migrationBuilder.DropTable(
                name: "reports",
                schema: "reporting-service");
        }
    }
}
