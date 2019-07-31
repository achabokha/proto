using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Embily.Models.Migrations
{
    public partial class programs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProgramId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    DateModified = table.Column<DateTime>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ProgramId = table.Column<string>(nullable: false),
                    Domain = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Settings = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.ProgramId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProgramId",
                table: "Users",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Programs_ProgramId",
                table: "Users",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "ProgramId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Programs_ProgramId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProgramId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "Users");
        }
    }
}
