using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrainingSystem.Data.migrations
{
    /// <inheritdoc />
    public partial class SeedDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "ComapnyId", "Adress", "Country", "Name" },
                values: new object[,]
                {
                    { new Guid("455cdf8b-9469-45e4-aa05-0c24b3541503"), "any adress at Maiami", "USA", "Tesla" },
                    { new Guid("92f04823-3b8f-48c4-a37f-32529f8724f5"), "any adress at Cairo", "Egypt", "Nasa" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyOfTheEmployeeId", "Name", "Position" },
                values: new object[,]
                {
                    { new Guid("3e0f6702-f265-4e61-912a-66140071974c"), 40, null, "John", "DEVOPS" },
                    { new Guid("5134eb68-3a7e-4042-a549-b92b87abbc03"), 50, null, "Mai", "Tester" },
                    { new Guid("67adda78-4608-42e7-9463-3773e0ffa4db"), 30, null, "Ahmed", "SE Engineer" },
                    { new Guid("7c3ea679-55e9-42fe-908d-65b5823f54c0"), 20, null, "Sara", "Product Owner" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "ComapnyId",
                keyValue: new Guid("455cdf8b-9469-45e4-aa05-0c24b3541503"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "ComapnyId",
                keyValue: new Guid("92f04823-3b8f-48c4-a37f-32529f8724f5"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("3e0f6702-f265-4e61-912a-66140071974c"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("5134eb68-3a7e-4042-a549-b92b87abbc03"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("67adda78-4608-42e7-9463-3773e0ffa4db"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("7c3ea679-55e9-42fe-908d-65b5823f54c0"));
        }
    }
}
