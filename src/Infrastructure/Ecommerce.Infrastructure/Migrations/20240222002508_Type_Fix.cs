using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Type_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShippingOperator",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeightOrder",
                table: "Orders",
                type: "INT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "shippingOperators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameService = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    TarifaShipping = table.Column<decimal>(type: "DECIMAL(20,2)", nullable: true),
                    NameShippingOperator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatorStatus = table.Column<bool>(type: "bit", nullable: true),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "INT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shippingOperators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shippingOperators_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "shippings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "INT", nullable: true),
                    OperatorId = table.Column<int>(type: "INT", nullable: true),
                    TotalShipping = table.Column<decimal>(type: "DECIMAL(20,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shippings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shippings_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_shippings_shippingOperators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "shippingOperators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_shippingOperators_CountryId",
                table: "shippingOperators",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_shippings_OperatorId",
                table: "shippings",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_shippings_OrderId",
                table: "shippings",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shippings");

            migrationBuilder.DropTable(
                name: "shippingOperators");

            migrationBuilder.DropColumn(
                name: "ShippingOperator",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WeightOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "AspNetUsers");
        }
    }
}
