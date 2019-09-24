using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ProductId" },
                values: new object[] { "84dc3fb9-757a-4cc4-8a00-558224ba9549", "Test Cat 1", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ProductId" },
                values: new object[] { "5b1f5211-32e7-4631-8d48-049bb2e70a6e", "Test Cat 2", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ProductId" },
                values: new object[] { "45f82d18-ccc4-432f-b3ad-609d98b56efc", "Test Cat 3", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "45f82d18-ccc4-432f-b3ad-609d98b56efc");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "5b1f5211-32e7-4631-8d48-049bb2e70a6e");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "84dc3fb9-757a-4cc4-8a00-558224ba9549");
        }
    }
}
