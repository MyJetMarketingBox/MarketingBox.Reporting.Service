using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class UpdateDepositMode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovedType",
                schema: "reporting-service",
                table: "registrations_details",
                newName: "UpdateMode");

            migrationBuilder.RenameIndex(
                name: "IX_registrations_details_ApprovedType",
                schema: "reporting-service",
                table: "registrations_details",
                newName: "IX_registrations_details_UpdateMode");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "reporting-service",
                table: "deposits",
                newName: "UpdateMode");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "reporting-service",
                table: "deposits",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.RenameColumn(
                name: "UpdateMode",
                schema: "reporting-service",
                table: "registrations_details",
                newName: "ApprovedType");

            migrationBuilder.RenameIndex(
                name: "IX_registrations_details_UpdateMode",
                schema: "reporting-service",
                table: "registrations_details",
                newName: "IX_registrations_details_ApprovedType");

            migrationBuilder.RenameColumn(
                name: "UpdateMode",
                schema: "reporting-service",
                table: "deposits",
                newName: "Type");
        }
    }
}
