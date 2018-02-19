using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GOC.Inventory.Infrastructure.Migrations
{
    public partial class AddBasicFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateUtc",
                table: "Items",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Items",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Inventories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateUtc",
                table: "Inventories",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Inventories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Vendors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateUtc",
                table: "Vendors",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Vendors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine1",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine2",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Companies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateUtc",
                table: "Companies",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Companies",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedDateUtc",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CreatedDateUtc",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CreatedDateUtc",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine1",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine2",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Address_State",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CreatedDateUtc",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Companies");
        }
    }
}
