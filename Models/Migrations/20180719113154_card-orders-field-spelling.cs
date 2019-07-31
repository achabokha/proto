using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Embily.Models.Migrations
{
    public partial class cardordersfieldspelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurcasheAmount",
                table: "CardOrders");

            migrationBuilder.AddColumn<double>(
                name: "PurchaseAmount",
                table: "CardOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseAmount",
                table: "CardOrders");

            migrationBuilder.AddColumn<double>(
                name: "PurcasheAmount",
                table: "CardOrders",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
