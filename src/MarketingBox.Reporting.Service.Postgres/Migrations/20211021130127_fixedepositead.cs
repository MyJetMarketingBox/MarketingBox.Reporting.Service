using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class fixedepositead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.DropIndex(
                name: "IX_deposits_LeadId",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.DropColumn(
                name: "DepositId",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "reporting-service",
                table: "leads",
                newName: "CrmStatus");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ConversionDate",
                schema: "reporting-service",
                table: "leads",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "reporting-service",
                table: "leads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DepositDate",
                schema: "reporting-service",
                table: "leads",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                schema: "reporting-service",
                table: "leads",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits",
                columns: new[] { "LeadId", "AffiliateId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.DropColumn(
                name: "ConversionDate",
                schema: "reporting-service",
                table: "leads");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "reporting-service",
                table: "leads");

            migrationBuilder.DropColumn(
                name: "DepositDate",
                schema: "reporting-service",
                table: "leads");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "reporting-service",
                table: "leads");

            migrationBuilder.RenameColumn(
                name: "CrmStatus",
                schema: "reporting-service",
                table: "leads",
                newName: "Type");

            migrationBuilder.AddColumn<long>(
                name: "DepositId",
                schema: "reporting-service",
                table: "deposits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits",
                column: "DepositId");

            migrationBuilder.CreateIndex(
                name: "IX_deposits_LeadId",
                schema: "reporting-service",
                table: "deposits",
                column: "LeadId");
        }
    }
}
