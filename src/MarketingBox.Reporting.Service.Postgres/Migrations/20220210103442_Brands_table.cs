using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class Brands_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brands",
                schema: "reporting-service",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Payout_Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    Payout_Currency = table.Column<int>(type: "integer", nullable: true),
                    Payout_Plan = table.Column<int>(type: "integer", nullable: true),
                    Revenue_Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    Revenue_Currency = table.Column<int>(type: "integer", nullable: true),
                    Revenue_Plan = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => new { x.Id, x.TenantId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "brands",
                schema: "reporting-service");
        }
    }
}
