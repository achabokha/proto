using Microsoft.EntityFrameworkCore.Migrations;

namespace Embily.Models.Migrations
{
    public partial class cardNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Applications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Accounts");
        }
    }
}
