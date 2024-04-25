using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixProductDimensioModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductDimensions_ProductDimensionId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductDimensionId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "width",
                table: "ProductDimensions",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "weight",
                table: "ProductDimensions",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "depth",
                table: "ProductDimensions",
                newName: "Depth");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductDimensions",
                type: "INT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductDimensions_ProductId",
                table: "ProductDimensions",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDimensions_Products_ProductId",
                table: "ProductDimensions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDimensions_Products_ProductId",
                table: "ProductDimensions");

            migrationBuilder.DropIndex(
                name: "IX_ProductDimensions_ProductId",
                table: "ProductDimensions");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductDimensions");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "ProductDimensions",
                newName: "width");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "ProductDimensions",
                newName: "weight");

            migrationBuilder.RenameColumn(
                name: "Depth",
                table: "ProductDimensions",
                newName: "depth");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductDimensionId",
                table: "Products",
                column: "ProductDimensionId",
                unique: true,
                filter: "[ProductDimensionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductDimensions_ProductDimensionId",
                table: "Products",
                column: "ProductDimensionId",
                principalTable: "ProductDimensions",
                principalColumn: "Id");
        }
    }
}
