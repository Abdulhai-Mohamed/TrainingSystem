using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingSystem.Data.migrations
{
    /// <inheritdoc />
    public partial class fixFKCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_CompanyOfTheEmployeeId",
                table: "Employees");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies_CompanyOfTheEmployeeId",
                table: "Employees",
                column: "CompanyOfTheEmployeeId",
                principalTable: "Companies",
                principalColumn: "ComapnyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_CompanyOfTheEmployeeId",
                table: "Employees");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies_CompanyOfTheEmployeeId",
                table: "Employees",
                column: "CompanyOfTheEmployeeId",
                principalTable: "Companies",
                principalColumn: "ComapnyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
