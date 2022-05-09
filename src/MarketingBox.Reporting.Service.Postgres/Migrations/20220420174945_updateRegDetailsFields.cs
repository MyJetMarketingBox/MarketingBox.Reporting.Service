using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class updateRegDetailsFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                schema: "reporting-service",
                table: "registrations_details",
                newName: "CountryAlfa2Code");

            migrationBuilder.RenameIndex(
                name: "IX_registrations_details_Country",
                schema: "reporting-service",
                table: "registrations_details",
                newName: "IX_registrations_details_CountryAlfa2Code");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "reporting-service",
                table: "registrations_details",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.RenameColumn(
                name: "CountryAlfa2Code",
                schema: "reporting-service",
                table: "registrations_details",
                newName: "Country");

            migrationBuilder.RenameIndex(
                name: "IX_registrations_details_CountryAlfa2Code",
                schema: "reporting-service",
                table: "registrations_details",
                newName: "IX_registrations_details_Country");
        }
    }
}
