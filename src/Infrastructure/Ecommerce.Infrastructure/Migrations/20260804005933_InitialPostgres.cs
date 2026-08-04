using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserAddress = table.Column<string>(type: "NVARCHAR(4000)", nullable: true),
                    City = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Region = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    PostalCode = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    UserName = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Country = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(90)", maxLength: 90, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    IdentityNumber = table.Column<string>(type: "text", nullable: true),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(90)", maxLength: 90, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Iso2 = table.Column<string>(type: "text", nullable: true),
                    Iso3 = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserAddress = table.Column<string>(type: "NVARCHAR(4000)", nullable: true),
                    City = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Region = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    PostalCode = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    UserName = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Country = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShoppingCartMasterId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "character varying(36)", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(36)", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "character varying(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(36)", nullable: false),
                    RoleId = table.Column<string>(type: "character varying(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(36)", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductCode = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    ProductName = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Description = table.Column<string>(type: "NVARCHAR(4000)", nullable: true),
                    typeProduct = table.Column<int>(type: "integer", nullable: false),
                    UnitToSell = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    UnitToBuy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    UnitToStore = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    UnitToProduction = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Currency = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(20,2)", nullable: true),
                    Rating = table.Column<int>(type: "INT", nullable: true),
                    RatingTotal = table.Column<int>(type: "INT", nullable: true),
                    ProviderName = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    SellerName = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    CountrySell = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    PurchaseCriteria = table.Column<int>(type: "integer", nullable: false),
                    Stock = table.Column<int>(type: "INT", nullable: true),
                    PurchaseLot = table.Column<int>(type: "INT", nullable: true),
                    PurchaseLeadTime = table.Column<int>(type: "INT", nullable: true),
                    PurchaseLeadTimeUnit = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    ReplenishmentPoint = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    SafetyStock = table.Column<int>(type: "INT", nullable: true),
                    ProductDimensionId = table.Column<int>(type: "INT", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "INT", nullable: false),
                    BarCode = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    QrCode = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "shippingOperators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameService = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    OrderId = table.Column<int>(type: "integer", nullable: true),
                    TarifaShipping = table.Column<decimal>(type: "numeric(20,2)", nullable: true),
                    NameShippingOperator = table.Column<string>(type: "text", nullable: true),
                    OperatorStatus = table.Column<bool>(type: "boolean", nullable: true),
                    CountryName = table.Column<string>(type: "text", nullable: true),
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
                name: "Taxs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Percentage = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CountryId = table.Column<int>(type: "INT", nullable: false),
                    ApplicationTax = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taxs_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BuyerName = table.Column<string>(type: "text", nullable: true),
                    BuyerUserName = table.Column<string>(type: "text", nullable: true),
                    OrderAddressId = table.Column<int>(type: "INT", nullable: true),
                    SubTotal = table.Column<decimal>(type: "numeric(20,2)", nullable: true),
                    orderStatus = table.Column<int>(type: "integer", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(20,2)", nullable: true),
                    PriceTax = table.Column<decimal>(type: "numeric(20,2)", nullable: true),
                    WeightOrder = table.Column<int>(type: "INT", nullable: true),
                    ShippingOperator = table.Column<string>(type: "text", nullable: true),
                    ShippingCost = table.Column<decimal>(type: "numeric(20,2)", nullable: true),
                    PaymentIntentId = table.Column<string>(type: "text", nullable: true),
                    ClientSecret = table.Column<string>(type: "text", nullable: true),
                    StripeApiKey = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_OrderAddresses_OrderAddressId",
                        column: x => x.OrderAddressId,
                        principalTable: "OrderAddresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductName = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(20,2)", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ProductPicture = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    ShoppingCartMasterId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShoppingCartId = table.Column<int>(type: "INT", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "NVARCHAR(4000)", nullable: true),
                    PublicCode = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "INT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDimensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Length = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Depth = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "INT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDimensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDimensions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "NVARCHAR(4000)", nullable: true),
                    ProductId = table.Column<int>(type: "INT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxByProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INT", nullable: false),
                    TaxId = table.Column<int>(type: "INT", nullable: false),
                    IsActivated = table.Column<bool>(type: "boolean", nullable: false),
                    Id = table.Column<int>(type: "INT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxByProducts", x => new { x.ProductId, x.TaxId });
                    table.ForeignKey(
                        name: "FK_TaxByProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxByProducts_Taxs_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Taxs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "INT", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(20,2)", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "INT", nullable: false),
                    ProductItemId = table.Column<int>(type: "integer", nullable: false),
                    productName = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "parTaxItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaxName = table.Column<string>(type: "text", nullable: true),
                    TaxPercentage = table.Column<decimal>(type: "numeric(20,2)", nullable: true),
                    MontoItem = table.Column<decimal>(type: "numeric(20,2)", nullable: true),
                    TotalMontoItem = table.Column<decimal>(type: "numeric(20,2)", nullable: true),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shippings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "INT", nullable: true),
                    OperatorId = table.Column<int>(type: "INT", nullable: true),
                    TotalShipping = table.Column<decimal>(type: "numeric(20,2)", nullable: true),
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderAddressId",
                table: "Orders",
                column: "OrderAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_parTaxItems_OrderId",
                table: "parTaxItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDimensions_ProductId",
                table: "ProductDimensions",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ShoppingCartId",
                table: "ShoppingCartItems",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxByProducts_TaxId",
                table: "TaxByProducts",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxs_CountryId",
                table: "Taxs",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "parTaxItems");

            migrationBuilder.DropTable(
                name: "ProductDimensions");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "shippings");

            migrationBuilder.DropTable(
                name: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "TaxByProducts");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "shippingOperators");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Taxs");

            migrationBuilder.DropTable(
                name: "OrderAddresses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
