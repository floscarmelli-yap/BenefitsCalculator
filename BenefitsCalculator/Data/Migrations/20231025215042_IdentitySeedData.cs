using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenefitsCalculator.Migrations
{
    /// <inheritdoc />
    public partial class IdentitySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "819a56ed-ca80-4784-ba99-e6d763f7ffbe", 0, "7d6a2cbb-c387-4ce9-9cd2-42e5a32f5409", "admin@benefitscalc.com", false, "Admin", "BenefitsCalc", false, null, null, null, null, null, false, "b7d5a7cc-bbd7-489e-a8a6-ad43a07a2880", false, "admin@benefitscalc.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "819a56ed-ca80-4784-ba99-e6d763f7ffbe");
        }
    }
}
