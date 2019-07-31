using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Embily.Models.Migrations
{
    public partial class shippingoptionspromocode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CardCost",
                table: "Applications",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PromoCode",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SelectedShippingOption",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ShippingCost",
                table: "Applications",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ShippingOptions",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardCost",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "PromoCode",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "SelectedShippingOption",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ShippingOptions",
                table: "Applications");
        }
    }
}
