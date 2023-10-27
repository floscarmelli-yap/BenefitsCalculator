using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenefitsCalculator.Migrations
{
    /// <inheritdoc />
    public partial class AddedBenefitsGroupIdProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "BenefitsHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "BenefitsHistories");
        }
    }
}
