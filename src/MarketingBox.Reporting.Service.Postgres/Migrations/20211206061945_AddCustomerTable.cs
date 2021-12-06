using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class AddCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "reporting-service",
                table: "affiliate_access",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "customer",
                schema: "reporting-service",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    FirstName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    LastName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Phone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Ip = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Country = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    AffiliateId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeposit = table.Column<bool>(type: "boolean", nullable: false),
                    DepositDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customer_Country",
                schema: "reporting-service",
                table: "customer",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_customer_CreatedDate",
                schema: "reporting-service",
                table: "customer",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_customer_Email",
                schema: "reporting-service",
                table: "customer",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_customer_IsDeposit",
                schema: "reporting-service",
                table: "customer",
                column: "IsDeposit");

            migrationBuilder.CreateIndex(
                name: "IX_customer_TenantId",
                schema: "reporting-service",
                table: "customer",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer",
                schema: "reporting-service");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "reporting-service",
                table: "affiliate_access",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
