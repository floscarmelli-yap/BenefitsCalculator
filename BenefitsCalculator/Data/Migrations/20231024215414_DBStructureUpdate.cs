using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenefitsCalculator.Migrations
{
    /// <inheritdoc />
    public partial class DBStructureUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BenefitsHistories_Consumers_ConsumerId",
                table: "BenefitsHistories");

            migrationBuilder.DropIndex(
                name: "IX_BenefitsHistories_ConsumerId",
                table: "BenefitsHistories");

            migrationBuilder.DropColumn(
                name: "BasicSalary",
                table: "BenefitsHistories");

            migrationBuilder.DropColumn(
                name: "ConsumerId",
                table: "BenefitsHistories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BenefitsHistories");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BenefitsHistories");

            migrationBuilder.DropColumn(
                name: "GuaranteedIssue",
                table: "BenefitsHistories");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "BenefitsHistories",
                newName: "BenefitsHistGroupId");

            migrationBuilder.CreateTable(
                name: "BenefitsHistGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsumerId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    GuaranteedIssue = table.Column<double>(type: "float", nullable: false),
                    BasicSalary = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitsHistGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenefitsHistGroups_Consumers_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "Consumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitsHistories_BenefitsHistGroupId",
                table: "BenefitsHistories",
                column: "BenefitsHistGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitsHistGroups_ConsumerId",
                table: "BenefitsHistGroups",
                column: "ConsumerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BenefitsHistories_BenefitsHistGroups_BenefitsHistGroupId",
                table: "BenefitsHistories",
                column: "BenefitsHistGroupId",
                principalTable: "BenefitsHistGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BenefitsHistories_BenefitsHistGroups_BenefitsHistGroupId",
                table: "BenefitsHistories");

            migrationBuilder.DropTable(
                name: "BenefitsHistGroups");

            migrationBuilder.DropIndex(
                name: "IX_BenefitsHistories_BenefitsHistGroupId",
                table: "BenefitsHistories");

            migrationBuilder.RenameColumn(
                name: "BenefitsHistGroupId",
                table: "BenefitsHistories",
                newName: "GroupId");

            migrationBuilder.AddColumn<double>(
                name: "BasicSalary",
                table: "BenefitsHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ConsumerId",
                table: "BenefitsHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "BenefitsHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BenefitsHistories",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "GuaranteedIssue",
                table: "BenefitsHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_BenefitsHistories_ConsumerId",
                table: "BenefitsHistories",
                column: "ConsumerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BenefitsHistories_Consumers_ConsumerId",
                table: "BenefitsHistories",
                column: "ConsumerId",
                principalTable: "Consumers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
