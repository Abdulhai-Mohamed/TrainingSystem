using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrainingSystem.Data.migrations
{
    /// <inheritdoc />
    public partial class SeedDataBaseByRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "131f1805-4fa2-4ae0-9a1b-b70cb631076f", "aab07c72-8812-4d7c-8de2-d19f8f2baaae", "Manager", "MANAGER" },
                    { "7de316c1-0553-411e-984b-db62bcd69a70", "2222e795-070d-4a62-ae9f-2e56e16bc0b1", "Adminstrator", "ADMINSTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "131f1805-4fa2-4ae0-9a1b-b70cb631076f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7de316c1-0553-411e-984b-db62bcd69a70");
        }
    }
}
