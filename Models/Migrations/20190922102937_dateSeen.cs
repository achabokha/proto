using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class dateSeen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessageSeen_ChatMessages_ChatMessageId",
                table: "ChatMessageSeen");

            migrationBuilder.RenameColumn(
                name: "ChatMessageId",
                table: "ChatMessageSeen",
                newName: "ChatGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessageSeen_ChatMessageId",
                table: "ChatMessageSeen",
                newName: "IX_ChatMessageSeen_ChatGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessageSeen_ChatGroups_ChatGroupId",
                table: "ChatMessageSeen",
                column: "ChatGroupId",
                principalTable: "ChatGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessageSeen_ChatGroups_ChatGroupId",
                table: "ChatMessageSeen");

            migrationBuilder.RenameColumn(
                name: "ChatGroupId",
                table: "ChatMessageSeen",
                newName: "ChatMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessageSeen_ChatGroupId",
                table: "ChatMessageSeen",
                newName: "IX_ChatMessageSeen_ChatMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessageSeen_ChatMessages_ChatMessageId",
                table: "ChatMessageSeen",
                column: "ChatMessageId",
                principalTable: "ChatMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
