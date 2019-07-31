using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Embily.Models.Migrations
{
    public partial class affiliatenormalized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AffiliateTokens_Token",
                table: "AffiliateTokens");

            migrationBuilder.DropIndex(
                name: "IX_AffiliateEmails_Email",
                table: "AffiliateEmails");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "AffiliateTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "NormalizedToken",
                table: "AffiliateTokens",
                type: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "AffiliateEmails",
                type: "nvarchar(450)",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AffiliateEmails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateTokens_NormalizedToken",
                table: "AffiliateTokens",
                column: "NormalizedToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateEmails_NormalizedEmail",
                table: "AffiliateEmails",
                column: "NormalizedEmail",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AffiliateTokens_NormalizedToken",
                table: "AffiliateTokens");

            migrationBuilder.DropIndex(
                name: "IX_AffiliateEmails_NormalizedEmail",
                table: "AffiliateEmails");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "AffiliateTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.DropColumn(
                name: "NormalizedToken",
                table: "AffiliateTokens");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "AffiliateEmails");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AffiliateEmails",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateTokens_Token",
                table: "AffiliateTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateEmails_Email",
                table: "AffiliateEmails",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }
    }
}
