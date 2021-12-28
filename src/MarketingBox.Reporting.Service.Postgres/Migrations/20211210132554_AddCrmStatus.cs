using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class AddCrmStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CrmStatus",
                schema: "reporting-service",
                table: "customer",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrmStatus",
                schema: "reporting-service",
                table: "customer");
        }
    }
}
