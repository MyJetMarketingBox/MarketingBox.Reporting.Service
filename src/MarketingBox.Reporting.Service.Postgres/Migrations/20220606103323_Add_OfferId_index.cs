using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class Add_OfferId_index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.AlterColumn<long>(
                name: "IntegrationId",
                schema: "reporting-service",
                table: "registrations_details",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "OfferId",
                schema: "reporting-service",
                table: "registrations_details",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_registrations_details_OfferId",
                schema: "reporting-service",
                table: "registrations_details",
                column: "OfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_registrations_details_OfferId",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropColumn(
                name: "OfferId",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.AlterColumn<long>(
                name: "IntegrationId",
                schema: "reporting-service",
                table: "registrations_details",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "reporting-service",
                table: "registrations_details",
                type: "text",
                nullable: true);
        }
    }
}
