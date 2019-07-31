using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Embily.Models.Migrations
{
    public partial class cardorders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardOrders",
                columns: table => new
                {
                    CardOrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CryptoAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CryptoAddressQRCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CryptoAmount = table.Column<double>(type: "float", nullable: false),
                    CryptoCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurcasheAmount = table.Column<double>(type: "float", nullable: false),
                    PurchaseCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardOrders", x => x.CardOrderId);
                    table.ForeignKey(
                        name: "FK_CardOrders_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardOrders_ApplicationId",
                table: "CardOrders",
                column: "ApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardOrders");
        }
    }
}
