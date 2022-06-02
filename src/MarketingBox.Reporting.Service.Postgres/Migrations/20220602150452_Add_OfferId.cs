using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class Add_OfferId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OfferId",
                schema: "reporting-service",
                table: "trackinglinks",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_trackinglinks_OfferId",
                schema: "reporting-service",
                table: "trackinglinks",
                column: "OfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_trackinglinks_OfferId",
                schema: "reporting-service",
                table: "trackinglinks");

            migrationBuilder.DropColumn(
                name: "OfferId",
                schema: "reporting-service",
                table: "trackinglinks");
        }
    }
}
