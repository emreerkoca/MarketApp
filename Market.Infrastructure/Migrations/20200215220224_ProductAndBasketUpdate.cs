using Microsoft.EntityFrameworkCore.Migrations;

namespace Article.Infrastructure.Migrations
{
    public partial class ProductAndBasketUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "BasketItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ActivationStatus",
                table: "Basket",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "BasketItem");

            migrationBuilder.DropColumn(
                name: "ActivationStatus",
                table: "Basket");
        }
    }
}
