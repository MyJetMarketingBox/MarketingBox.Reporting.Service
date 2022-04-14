using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class TrackingLinkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "trackinglinks",
                schema: "reporting-service",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ClickId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    AffiliateId = table.Column<long>(type: "bigint", nullable: false),
                    Link = table.Column<string>(type: "text", nullable: false),
                    LinkParameterValues_Language = table.Column<string>(type: "text", nullable: true),
                    LinkParameterValues_MPC_1 = table.Column<string>(type: "text", nullable: true),
                    LinkParameterValues_MPC_2 = table.Column<string>(type: "text", nullable: true),
                    LinkParameterValues_MPC_3 = table.Column<string>(type: "text", nullable: true),
                    LinkParameterValues_MPC_4 = table.Column<string>(type: "text", nullable: true),
                    LinkParameterNames_ClickId = table.Column<string>(type: "text", nullable: true),
                    LinkParameterNames_Language = table.Column<string>(type: "text", nullable: true),
                    LinkParameterNames_MPC_1 = table.Column<string>(type: "text", nullable: true),
                    LinkParameterNames_MPC_2 = table.Column<string>(type: "text", nullable: true),
                    LinkParameterNames_MPC_3 = table.Column<string>(type: "text", nullable: true),
                    LinkParameterNames_MPC_4 = table.Column<string>(type: "text", nullable: true),
                    UniqueId = table.Column<string>(type: "text", nullable: false),
                    RegistrationId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trackinglinks", x => new { x.Id, x.ClickId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_trackinglinks_ClickId",
                schema: "reporting-service",
                table: "trackinglinks",
                column: "ClickId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trackinglinks_UniqueId",
                schema: "reporting-service",
                table: "trackinglinks",
                column: "UniqueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trackinglinks",
                schema: "reporting-service");
        }
    }
}
