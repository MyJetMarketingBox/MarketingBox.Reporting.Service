using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class RegistrationDetails_CountryCode_to_CountryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_registrations_details_CountryAlfa2Code",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropColumn(
                name: "CountryAlfa2Code",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                schema: "reporting-service",
                table: "registrations_details",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_BrandId",
                schema: "reporting-service",
                table: "registrations_details",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_CampaignId",
                schema: "reporting-service",
                table: "registrations_details",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_CountryId",
                schema: "reporting-service",
                table: "registrations_details",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_CrmStatus",
                schema: "reporting-service",
                table: "registrations_details",
                column: "CrmStatus");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_FirstName",
                schema: "reporting-service",
                table: "registrations_details",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_IntegrationId",
                schema: "reporting-service",
                table: "registrations_details",
                column: "IntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_LastName",
                schema: "reporting-service",
                table: "registrations_details",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_Phone",
                schema: "reporting-service",
                table: "registrations_details",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_RegistrationId",
                schema: "reporting-service",
                table: "registrations_details",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_Status",
                schema: "reporting-service",
                table: "registrations_details",
                column: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_registrations_details_BrandId",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropIndex(
                name: "IX_registrations_details_CampaignId",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropIndex(
                name: "IX_registrations_details_CountryId",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropIndex(
                name: "IX_registrations_details_CrmStatus",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropIndex(
                name: "IX_registrations_details_FirstName",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropIndex(
                name: "IX_registrations_details_IntegrationId",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropIndex(
                name: "IX_registrations_details_LastName",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropIndex(
                name: "IX_registrations_details_Phone",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropIndex(
                name: "IX_registrations_details_RegistrationId",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropIndex(
                name: "IX_registrations_details_Status",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropColumn(
                name: "CountryId",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.AddColumn<string>(
                name: "CountryAlfa2Code",
                schema: "reporting-service",
                table: "registrations_details",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_CountryAlfa2Code",
                schema: "reporting-service",
                table: "registrations_details",
                column: "CountryAlfa2Code");
        }
    }
}
