using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class Migration080222 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DepositDate",
                schema: "reporting-service",
                table: "registrations_details",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepositDate",
                schema: "reporting-service",
                table: "registrations_details");
        }
    }
}
