using Ecommerce.Application.Features.Products.Queries.GetInventoryProductByCode;
using Ecommerce.Application.Features.Products.Queries.GetInventoryProductCatalog;
using Ecommerce.Application.Features.Products.Queries.Vms.Inventory;
using Ecommerce.Application.Models.Authorization;
using Ecommerce.Domain;

namespace Ecommerce.UnitTests;

public class InventoryQueryTests
{
    // ── GetInventoryProductByCodeQuery ─────────────────────────────────────────

    [Fact]
    public void GetInventoryProductByCodeQueryRejectsNullOrWhitespaceCode()
    {
        Assert.Throws<ArgumentException>(() => new GetInventoryProductByCodeQuery(null!));
        Assert.Throws<ArgumentException>(() => new GetInventoryProductByCodeQuery("   "));
        Assert.Throws<ArgumentException>(() => new GetInventoryProductByCodeQuery(string.Empty));
    }

    [Fact]
    public void GetInventoryProductByCodeQueryTrimsCode()
    {
        var query = new GetInventoryProductByCodeQuery("  P-001  ");

        Assert.Equal("P-001", query.ProductCode);
    }

    // ── GetInventoryProductByCodeQueryHandler.MapToVm ─────────────────────────

    [Fact]
    public void MapToVmMapsAllRequiredFields()
    {
        var category = new Category { Id = 5, Name = "Electronics" };
        var product = new Product
        {
            ProductCode = "P-001",
            ProductName = "Widget",
            Description = "A widget",
            Price = 9.99m,
            Currency = "USD",
            Stock = 42,
            UnitToSell = "unit",
            PurchaseLeadTime = 3,
            PurchaseLeadTimeUnit = "days",
            Status = ProductStatus.Active,
            Category = category,
        };
        product.Id = 7;

        var vm = GetInventoryProductByCodeQueryHandler.MapToVm(product);

        Assert.Equal("P-001", vm.ProductCode);
        Assert.Equal(7, vm.ProductId);
        Assert.Equal("Widget", vm.ProductName);
        Assert.Equal("A widget", vm.Description);
        Assert.Equal("Electronics", vm.Category);
        Assert.Equal(9.99m, vm.Price);
        Assert.Equal("USD", vm.Currency);
        Assert.Equal(42, vm.Stock);
        Assert.Equal("unit", vm.UnitToSell);
        Assert.Equal(3, vm.PurchaseLeadTime);
        Assert.Equal("days", vm.PurchaseLeadTimeUnit);
        Assert.Equal(ProductStatus.Active, vm.Status);
    }

    [Fact]
    public void MapToVmHandlesNullCategory()
    {
        var product = new Product
        {
            ProductCode = "P-002",
            ProductName = "Gadget",
            Status = ProductStatus.Active,
        };

        var vm = GetInventoryProductByCodeQueryHandler.MapToVm(product);

        Assert.Null(vm.Category);
    }

    // ── GetInventoryProductCatalogQuery ────────────────────────────────────────

    [Fact]
    public void GetInventoryProductCatalogQueryDefaultsPageToOne()
    {
        var query = new GetInventoryProductCatalogQuery();

        Assert.Equal(1, query.PageIndex);
    }

    // ── Role constants ─────────────────────────────────────────────────────────

    [Fact]
    public void SupplierIntegrationRoleConstantIsDefined()
    {
        Assert.Equal("SUPPLIER_INTEGRATION", Role.SUPPLIER_INTEGRATION);
    }

    // ── InventoryProductVm ────────────────────────────────────────────────────

    [Fact]
    public void InventoryProductVmExposesContractFields()
    {
        var vm = new InventoryProductVm
        {
            ProductCode = "P-001",
            ProductId = 1,
            ProductName = "Widget",
            Description = "desc",
            Category = "Electronics",
            Price = 10m,
            Currency = "EUR",
            Stock = 5,
            UnitToSell = "unit",
            PurchaseLeadTime = 2,
            PurchaseLeadTimeUnit = "days",
            Status = ProductStatus.Active,
        };

        Assert.NotNull(vm.ProductCode);
        Assert.NotNull(vm.ProductName);
        Assert.Equal(ProductStatus.Active, vm.Status);
    }
}
