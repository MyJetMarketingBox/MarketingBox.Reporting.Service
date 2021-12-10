using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class UpdateCustomerKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_customer",
                schema: "reporting-service",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "reporting-service",
                table: "customer");

            migrationBuilder.AlterColumn<string>(
                name: "UId",
                schema: "reporting-service",
                table: "customer",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_customer",
                schema: "reporting-service",
                table: "customer",
                column: "UId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_customer",
                schema: "reporting-service",
                table: "customer");

            migrationBuilder.AlterColumn<string>(
                name: "UId",
                schema: "reporting-service",
                table: "customer",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                schema: "reporting-service",
                table: "customer",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_customer",
                schema: "reporting-service",
                table: "customer",
                column: "Id");
        }
    }
}
