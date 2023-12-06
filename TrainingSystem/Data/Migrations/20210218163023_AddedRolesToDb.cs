using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingSystem.Data.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0ff6387e-a71a-4bbe-88e0-7c0760f4d43d", "7606b59a-0532-4eae-aaaf-7421a369a3a4", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c49fc338-64e5-43f3-80b5-a48ed33e3344", "a8ed838b-3fde-4343-a4cc-80fae0a75d89", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ff6387e-a71a-4bbe-88e0-7c0760f4d43d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c49fc338-64e5-43f3-80b5-a48ed33e3344");
        }
    }
}
