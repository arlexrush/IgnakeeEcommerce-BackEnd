using Ecommerce.Domain;

namespace Ecommerce.UnitTests;

public class ProductTests
{
    [Fact]
    public void SetBasicInformationStoresRequiredValues()
    {
        var product = new Product();

        product.SetBasicInformation("P-001", "Coffee", "Ground coffee");

        Assert.Equal("P-001", product.ProductCode);
        Assert.Equal("Coffee", product.ProductName);
        Assert.Equal("Ground coffee", product.Description);
    }

    [Theory]
    [InlineData(null, "Coffee")]
    [InlineData("P-001", null)]
    [InlineData(" ", "Coffee")]
    [InlineData("P-001", " ")]
    public void SetBasicInformationRejectsMissingRequiredValues(string? productCode, string? productName)
    {
        var product = new Product();

        Assert.Throws<ArgumentException>(() => product.SetBasicInformation(productCode, productName, null));
    }

    [Fact]
    public void SetPriceStoresNonNegativePrice()
    {
        var product = new Product();

        product.SetPrice(12.50m);

        Assert.Equal(12.50m, product.Price);
    }

    [Fact]
    public void SetPriceRejectsNegativePrice()
    {
        var product = new Product();

        Assert.Throws<ArgumentOutOfRangeException>(() => product.SetPrice(-0.01m));
    }

    [Fact]
    public void AddStockIncreasesAvailableStock()
    {
        var product = new Product { Stock = 2 };

        product.AddStock(3);

        Assert.Equal(5, product.Stock);
    }

    [Fact]
    public void ReserveStockReducesAvailableStock()
    {
        var product = new Product { Stock = 5 };

        product.ReserveStock(2);

        Assert.Equal(3, product.Stock);
    }

    [Fact]
    public void ReserveStockRejectsInsufficientStock()
    {
        var product = new Product { Stock = 1 };

        Assert.Throws<InvalidOperationException>(() => product.ReserveStock(2));
    }
}
