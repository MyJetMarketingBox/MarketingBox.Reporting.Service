using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class Fix_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payout_Amount",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "Payout_Currency",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "Payout_Plan",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "Revenue_Amount",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "Revenue_Currency",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "Revenue_Plan",
                schema: "reporting-service",
                table: "brands");

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

            migrationBuilder.AddColumn<int>(
                name: "RevenuePlan",
                schema: "reporting-service",
                table: "brands",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "RevenuePlan",
                schema: "reporting-service",
                table: "brands");

            migrationBuilder.AddColumn<decimal>(
                name: "Payout_Amount",
                schema: "reporting-service",
                table: "brands",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Payout_Currency",
                schema: "reporting-service",
                table: "brands",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Payout_Plan",
                schema: "reporting-service",
                table: "brands",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Revenue_Amount",
                schema: "reporting-service",
                table: "brands",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Revenue_Currency",
                schema: "reporting-service",
                table: "brands",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Revenue_Plan",
                schema: "reporting-service",
                table: "brands",
                type: "integer",
                nullable: true);
        }
    }
}
