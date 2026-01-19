using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoAnalyzer.Portfolio.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedHoldingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BuyingPrice",
                table: "Holdings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyingPrice",
                table: "Holdings");
        }
    }
}
