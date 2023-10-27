using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenefitsCalculator.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Setups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuaranteedIssue = table.Column<double>(type: "float", nullable: false),
                    MaxAgeLimit = table.Column<int>(type: "int", nullable: false),
                    MinAgeLimit = table.Column<int>(type: "int", nullable: false),
                    MinRange = table.Column<int>(type: "int", nullable: false),
                    MaxRange = table.Column<int>(type: "int", nullable: false),
                    Increments = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Consumers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BasicSalary = table.Column<double>(type: "float", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "Date", nullable: true),
                    SetupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consumers_Setups_SetupId",
                        column: x => x.SetupId,
                        principalTable: "Setups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BenefitsHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Multiple = table.Column<int>(type: "int", nullable: false),
                    AmountQuotation = table.Column<double>(type: "float", nullable: false),
                    PendedAmount = table.Column<double>(type: "float", nullable: false),
                    BenefitsStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "Date", nullable: false),
                    ConsumerId = table.Column<int>(type: "int", nullable: false),
                    GuaranteedIssue = table.Column<double>(type: "float", nullable: false),
                    BasicSalary = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitsHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenefitsHistories_Consumers_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "Consumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitsHistories_ConsumerId",
                table: "BenefitsHistories",
                column: "ConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_SetupId",
                table: "Consumers",
                column: "SetupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BenefitsHistories");

            migrationBuilder.DropTable(
                name: "Consumers");

            migrationBuilder.DropTable(
                name: "Setups");
        }
    }
}
