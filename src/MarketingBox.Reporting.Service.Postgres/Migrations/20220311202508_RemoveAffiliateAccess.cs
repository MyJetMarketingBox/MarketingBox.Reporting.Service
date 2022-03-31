using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class RemoveAffiliateAccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "affiliate_access",
                schema: "reporting-service");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "affiliate_access",
                schema: "reporting-service",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    AffiliateId = table.Column<long>(type: "bigint", nullable: false),
                    MasterAffiliateId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_affiliate_access", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_affiliate_access_AffiliateId",
                schema: "reporting-service",
                table: "affiliate_access",
                column: "AffiliateId");

            migrationBuilder.CreateIndex(
                name: "IX_affiliate_access_MasterAffiliateId_AffiliateId",
                schema: "reporting-service",
                table: "affiliate_access",
                columns: new[] { "MasterAffiliateId", "AffiliateId" },
                unique: true);
        }
    }
}
