using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addParTaxItemEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "parTaxItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxPercentage = table.Column<decimal>(type: "DECIMAL(20,2)", nullable: true),
                    MontoItem = table.Column<decimal>(type: "DECIMAL(20,2)", nullable: true),
                    TotalMontoItem = table.Column<decimal>(type: "DECIMAL(20,2)", nullable: true),
                    OrderId = table.Column<int>(type: "INT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parTaxItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_parTaxItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_parTaxItems_OrderId",
                table: "parTaxItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parTaxItems");
        }
    }
}
