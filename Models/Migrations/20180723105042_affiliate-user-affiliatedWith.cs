using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Embily.Models.Migrations
{
    public partial class affiliateuseraffiliatedWith : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInvite",
                table: "AffiliateEmails");

            migrationBuilder.DropColumn(
                name: "TokenUsed",
                table: "AffiliateEmails");

            migrationBuilder.AddColumn<string>(
                name: "AffiliateTokenUsed",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AffiliatedWithUserId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AffiliatedWithUserId",
                table: "Users",
                column: "AffiliatedWithUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_AffiliatedWithUserId",
                table: "Users",
                column: "AffiliatedWithUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_AffiliatedWithUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AffiliatedWithUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AffiliateTokenUsed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AffiliatedWithUserId",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "IsInvite",
                table: "AffiliateEmails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TokenUsed",
                table: "AffiliateEmails",
                nullable: true);
        }
    }
}
