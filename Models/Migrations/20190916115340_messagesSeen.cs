using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class messagesSeen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateSeen",
                table: "ChatMessages");

            migrationBuilder.AddColumn<int>(
                name: "UnreadMessages",
                table: "Participants",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChatMessageSeen",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    DateSeen = table.Column<DateTime>(nullable: true),
                    ChatMessageId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessageSeen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessageSeen_ChatMessages_ChatMessageId",
                        column: x => x.ChatMessageId,
                        principalTable: "ChatMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessageSeen_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessageSeen_ChatMessageId",
                table: "ChatMessageSeen",
                column: "ChatMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessageSeen_UserId",
                table: "ChatMessageSeen",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessageSeen");

            migrationBuilder.DropColumn(
                name: "UnreadMessages",
                table: "Participants");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSeen",
                table: "ChatMessages",
                nullable: true);
        }
    }
}
