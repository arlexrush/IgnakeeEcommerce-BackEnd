using System.Text.Json;
using Ecommerce.Application.Features.Inventory.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;

namespace Ecommerce.UnitTests;

public class InventoryContractSerializationTests
{
    private static readonly JsonSerializerOptions WebJsonOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task ProductFixtureMatchesThePublicInventoryContract()
    {
        var fixture = await LoadFixtureAsync("inventory-product-contract.json");
        var product = JsonSerializer.Deserialize<InventoryProductVm>(fixture, WebJsonOptions);

        Assert.NotNull(product);
        Assert.Equal("SKU-IPHONE-001", product.ProductCode);
        Assert.True(product.IsAvailableForSale);
        Assert.Equal("Active", product.Status);
    }

    [Fact]
    public async Task CatalogFixtureMatchesThePublicInventoryContract()
    {
        var fixture = await LoadFixtureAsync("inventory-catalog-contract.json");
        var catalog = JsonSerializer.Deserialize<PaginationVm<InventoryProductVm>>(fixture, WebJsonOptions);

        Assert.NotNull(catalog);
        Assert.Equal(50, catalog.PageSize);
        Assert.Single(catalog.Data!);
    }

    [Fact]
    public void InventoryContractUsesCamelCaseAndTextStatus()
    {
        var product = new InventoryProductVm
        {
            ProductCode = "SKU-IPHONE-001",
            IsAvailableForSale = true,
            Status = "Active"
        };

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(product, WebJsonOptions));

        Assert.Equal("SKU-IPHONE-001", document.RootElement.GetProperty("productCode").GetString());
        Assert.True(document.RootElement.GetProperty("isAvailableForSale").GetBoolean());
        Assert.Equal("Active", document.RootElement.GetProperty("status").GetString());
    }

    private static Task<string> LoadFixtureAsync(string fileName)
    {
        var fixturePath = Path.Combine(AppContext.BaseDirectory, "Fixtures", fileName);
        return File.ReadAllTextAsync(fixturePath);
    }
}