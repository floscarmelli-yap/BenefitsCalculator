using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenefitsCalculator.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Consumers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Consumers",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "BasicSalary",
                table: "Consumers",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Consumers",
                type: "Date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.AlterColumn<double>(
                name: "BasicSalary",
                table: "Consumers",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Consumers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
