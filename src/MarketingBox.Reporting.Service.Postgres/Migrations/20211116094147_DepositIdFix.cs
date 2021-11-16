using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class DepositIdFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits",
                column: "RegistrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits",
                columns: new[] { "RegistrationId", "AffiliateId" });
        }
    }
}
