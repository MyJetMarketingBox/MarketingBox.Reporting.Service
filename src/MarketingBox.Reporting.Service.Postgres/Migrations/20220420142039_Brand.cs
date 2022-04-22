using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class Brand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayoutAmount",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "PayoutCurrency",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "PayoutPlan",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "RevenueAmount",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "RevenueCurrency",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.RenameColumn(
                name: "RevenuePlan",
                schema: "reporting-service",
                table: "brands",
                newName: "IntegrationType");

            migrationBuilder.AddColumn<long>(
                name: "IntegrationId",
                schema: "reporting-service",
                table: "brands",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntegrationId",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.RenameColumn(
                name: "IntegrationType",
                schema: "reporting-service",
                table: "brands",
                newName: "RevenuePlan");

            migrationBuilder.AddColumn<decimal>(
                name: "PayoutAmount",
                schema: "reporting-service",
                table: "brands",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PayoutCurrency",
                schema: "reporting-service",
                table: "brands",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayoutPlan",
                schema: "reporting-service",
                table: "brands",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "RevenueAmount",
                schema: "reporting-service",
                table: "brands",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RevenueCurrency",
                schema: "reporting-service",
                table: "brands",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
