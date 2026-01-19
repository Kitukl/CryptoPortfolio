using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoAnalyzer.Portfolio.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Holdings",
                newName: "UserEmail");

            migrationBuilder.RenameIndex(
                name: "IX_Holdings_UserId",
                table: "Holdings",
                newName: "IX_Holdings_UserEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "Holdings",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Holdings_UserEmail",
                table: "Holdings",
                newName: "IX_Holdings_UserId");
        }
    }
}
