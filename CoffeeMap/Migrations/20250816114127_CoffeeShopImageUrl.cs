using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeMap.Migrations
{
    /// <inheritdoc />
    public partial class CoffeeShopImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Website",
                table: "CoffeeShops");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "CoffeeShops",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
