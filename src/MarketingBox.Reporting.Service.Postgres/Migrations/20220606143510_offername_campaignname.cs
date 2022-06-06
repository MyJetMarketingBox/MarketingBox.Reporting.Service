using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class offername_campaignname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CampaignId",
                schema: "reporting-service",
                table: "registrations_details",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "CampaignName",
                schema: "reporting-service",
                table: "registrations_details",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfferName",
                schema: "reporting-service",
                table: "registrations_details",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CampaignName",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.DropColumn(
                name: "OfferName",
                schema: "reporting-service",
                table: "registrations_details");

            migrationBuilder.AlterColumn<long>(
                name: "CampaignId",
                schema: "reporting-service",
                table: "registrations_details",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
