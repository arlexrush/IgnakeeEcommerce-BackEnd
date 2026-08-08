using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBehaviorProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BehaviorProfiles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    HasConsented = table.Column<bool>(type: "boolean", nullable: false),
                    ConsentUpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CatalogViews = table.Column<int>(type: "integer", nullable: false),
                    ProductViews = table.Column<int>(type: "integer", nullable: false),
                    CartAdditions = table.Column<int>(type: "integer", nullable: false),
                    CheckoutStarts = table.Column<int>(type: "integer", nullable: false),
                    LastActivityAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviorProfiles", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessedBehaviorMessages",
                columns: table => new
                {
                    MessageId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    EventType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ContractVersion = table.Column<int>(type: "integer", nullable: false),
                    ProcessedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedBehaviorMessages", x => x.MessageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BehaviorProfiles");

            migrationBuilder.DropTable(
                name: "ProcessedBehaviorMessages");
        }
    }
}
