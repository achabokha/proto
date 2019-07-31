using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Embily.Models.Migrations
{
    public partial class affiliateupdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AffiliateTokens",
                table: "AffiliateTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AffiliateEmails",
                table: "AffiliateEmails");

            migrationBuilder.DropColumn(
                name: "AffiliateTokensId",
                table: "AffiliateTokens");

            migrationBuilder.DropColumn(
                name: "AffiliateEmailsId",
                table: "AffiliateEmails");

            migrationBuilder.AddColumn<string>(
                name: "AffiliateTokenId",
                table: "AffiliateTokens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AffiliateEmailId",
                table: "AffiliateEmails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsInvite",
                table: "AffiliateEmails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TokenUsed",
                table: "AffiliateEmails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AffiliateTokens",
                table: "AffiliateTokens",
                column: "AffiliateTokenId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AffiliateEmails",
                table: "AffiliateEmails",
                column: "AffiliateEmailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AffiliateTokens",
                table: "AffiliateTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AffiliateEmails",
                table: "AffiliateEmails");

            migrationBuilder.DropColumn(
                name: "AffiliateTokenId",
                table: "AffiliateTokens");

            migrationBuilder.DropColumn(
                name: "AffiliateEmailId",
                table: "AffiliateEmails");

            migrationBuilder.DropColumn(
                name: "IsInvite",
                table: "AffiliateEmails");

            migrationBuilder.DropColumn(
                name: "TokenUsed",
                table: "AffiliateEmails");

            migrationBuilder.AddColumn<string>(
                name: "AffiliateTokensId",
                table: "AffiliateTokens",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AffiliateEmailsId",
                table: "AffiliateEmails",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AffiliateTokens",
                table: "AffiliateTokens",
                column: "AffiliateTokensId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AffiliateEmails",
                table: "AffiliateEmails",
                column: "AffiliateEmailsId");
        }
    }
}
