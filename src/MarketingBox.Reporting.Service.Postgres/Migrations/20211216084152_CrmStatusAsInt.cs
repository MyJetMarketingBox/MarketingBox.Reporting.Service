using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class CrmStatusAsInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandStatus",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.AddColumn<int>(
                name: "CrmStatus",
                schema: "reporting-service",
                table: "deposits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CrmStatus",
                schema: "reporting-service",
                table: "customer",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrmStatus",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.AddColumn<string>(
                name: "BrandStatus",
                schema: "reporting-service",
                table: "deposits",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CrmStatus",
                schema: "reporting-service",
                table: "customer",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
