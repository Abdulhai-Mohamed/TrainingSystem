using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingSystem.Data.migrations
{
    /// <inheritdoc />
    public partial class addFkOfCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdCompanyOfTheEmployee",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("3e0f6702-f265-4e61-912a-66140071974c"),
                column: "IdCompanyOfTheEmployee",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("5134eb68-3a7e-4042-a549-b92b87abbc03"),
                column: "IdCompanyOfTheEmployee",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("67adda78-4608-42e7-9463-3773e0ffa4db"),
                column: "IdCompanyOfTheEmployee",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("7c3ea679-55e9-42fe-908d-65b5823f54c0"),
                column: "IdCompanyOfTheEmployee",
                value: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCompanyOfTheEmployee",
                table: "Employees");
        }
    }
}
