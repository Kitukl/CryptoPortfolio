using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoAnalyzer.Portfolio.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TrueNamings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BuyingPrice",
                table: "Holdings",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "AveragePrice",
                table: "Holdings",
                newName: "PricePerUnit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Holdings",
                newName: "BuyingPrice");

            migrationBuilder.RenameColumn(
                name: "PricePerUnit",
                table: "Holdings",
                newName: "AveragePrice");
        }
    }
}
