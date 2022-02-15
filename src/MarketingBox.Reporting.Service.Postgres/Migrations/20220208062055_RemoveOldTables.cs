using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class RemoveOldTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Sequence",
                schema: "reporting-service",
                table: "registrations_details");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Sequence",
                schema: "reporting-service",
                table: "registrations_details",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "deposits",
                schema: "reporting-service",
                columns: table => new
                {
                    RegistrationId = table.Column<long>(type: "bigint", nullable: false),
                    AffiliateId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    ConversionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CrmStatus = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    IntegrationId = table.Column<long>(type: "bigint", nullable: false),
                    RegisterDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Sequence = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: true),
                    UniqueId = table.Column<string>(type: "text", nullable: true),
                    UpdateMode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposits", x => x.RegistrationId);
                });

            migrationBuilder.CreateTable(
                name: "registrations",
                schema: "reporting-service",
                columns: table => new
                {
                    RegistrationId = table.Column<long>(type: "bigint", nullable: false),
                    AffiliateId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    ConversionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CrmStatus = table.Column<int>(type: "integer", nullable: false),
                    DepositDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    IntegrationId = table.Column<long>(type: "bigint", nullable: false),
                    Ip = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Sequence = table.Column<long>(type: "bigint", nullable: false),
                    So = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Sub = table.Column<string>(type: "text", nullable: true),
                    Sub1 = table.Column<string>(type: "text", nullable: true),
                    Sub10 = table.Column<string>(type: "text", nullable: true),
                    Sub2 = table.Column<string>(type: "text", nullable: true),
                    Sub3 = table.Column<string>(type: "text", nullable: true),
                    Sub4 = table.Column<string>(type: "text", nullable: true),
                    Sub5 = table.Column<string>(type: "text", nullable: true),
                    Sub6 = table.Column<string>(type: "text", nullable: true),
                    Sub7 = table.Column<string>(type: "text", nullable: true),
                    Sub8 = table.Column<string>(type: "text", nullable: true),
                    Sub9 = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: true),
                    UniqueId = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
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
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IntegrationId = table.Column<long>(type: "bigint", nullable: false),
                    Payout = table.Column<decimal>(type: "numeric", nullable: false),
                    Revenue = table.Column<decimal>(type: "numeric", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: true),
                    UniqueId = table.Column<string>(type: "text", nullable: true)
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
    }
}
