using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Inventory.Queries.GetInventoryProductByCode;
using Ecommerce.Application.Features.Inventory.Queries.PaginationInventoryProducts;
using Ecommerce.Application.Mapping;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Ecommerce.IntegrationTests;

public class InventoryQueryTests
{
    [Fact]
    public async Task GetInventoryProductByCodeReturnsMappedActiveProduct()
    {
        await using var context = CreateContext();
        await SeedAsync(context);
        var handler = new GetInventoryProductByCodeQueryHandler(new UnitOfWork(context), CreateMapper());

        var result = await handler.Handle(new GetInventoryProductByCodeQuery("SKU-001"), CancellationToken.None);

        Assert.Equal("SKU-001", result.ProductCode);
        Assert.Equal(1, result.ProductId);
        Assert.Equal("Coffee", result.ProductName);
        Assert.Equal("Beverages", result.Category);
        Assert.Equal(12.5m, result.Price);
        Assert.Equal("USD", result.Currency);
        Assert.True(result.IsAvailableForSale);
        Assert.Equal("Active", result.Status);
    }

    [Fact]
    public async Task GetInventoryProductByCodeUsesDeterministicFallbackWhenProductCodeIsMissing()
    {
        await using var context = CreateContext();
        await SeedAsync(context);
        var handler = new GetInventoryProductByCodeQueryHandler(new UnitOfWork(context), CreateMapper());

        var result = await handler.Handle(new GetInventoryProductByCodeQuery("product-2"), CancellationToken.None);

        Assert.Equal("product-2", result.ProductCode);
        Assert.Equal(2, result.ProductId);
        Assert.Equal("Tea", result.ProductName);
    }

    [Fact]
    public async Task GetInventoryProductByCodeReturnsUnavailableWhenStockIsNull()
    {
        await using var context = CreateContext();
        await SeedAsync(context);
        var handler = new GetInventoryProductByCodeQueryHandler(new UnitOfWork(context), CreateMapper());

        var result = await handler.Handle(new GetInventoryProductByCodeQuery("SKU-004"), CancellationToken.None);

        Assert.False(result.IsAvailableForSale);
    }

    [Fact]
    public async Task GetInventoryProductByCodeRejectsInactiveProducts()
    {
        await using var context = CreateContext();
        await SeedAsync(context);
        var handler = new GetInventoryProductByCodeQueryHandler(new UnitOfWork(context), CreateMapper());

        await Assert.ThrowsAsync<NoFoundException>(() =>
            handler.Handle(new GetInventoryProductByCodeQuery("SKU-003"), CancellationToken.None));
    }

    [Fact]
    public async Task GetInventoryProductByCodeRejectsUnknownProducts()
    {
        await using var context = CreateContext();
        await SeedAsync(context);
        var handler = new GetInventoryProductByCodeQueryHandler(new UnitOfWork(context), CreateMapper());

        await Assert.ThrowsAsync<NoFoundException>(() =>
            handler.Handle(new GetInventoryProductByCodeQuery("SKU-404"), CancellationToken.None));
    }

    [Fact]
    public async Task PaginationInventoryProductsOnlyReturnsActiveProductsMatchingFilter()
    {
        await using var context = CreateContext();
        await SeedAsync(context);
        var handler = new PaginationInventoryProductsQueryHandler(new UnitOfWork(context), CreateMapper());

        var result = await handler.Handle(new PaginationInventoryProductsQuery
        {
            Search = "Coffee",
            CategoryId = 1,
            PageIndex = 1,
            PageSize = 10
        }, CancellationToken.None);

        Assert.Equal(1, result.Count);
        Assert.Single(result.Data!);
        Assert.Equal("SKU-001", result.Data![0].ProductCode);
        Assert.Equal(1, result.ResultByPage);
        Assert.Equal(1, result.PageCount);
    }

    private static IMapper CreateMapper()
    {
        return new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance).CreateMapper();
    }

    private static EcommerceDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<EcommerceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new EcommerceDbContext(options);
    }

    private static async Task SeedAsync(EcommerceDbContext context)
    {
        var category = new Category
        {
            Id = 1,
            Name = "Beverages"
        };

        await context.Categories!.AddAsync(category);
        await context.Products!.AddRangeAsync(
            new Product
            {
                Id = 1,
                ProductCode = "SKU-001",
                ProductName = "Coffee",
                Description = "Dark roast coffee",
                CategoryId = 1,
                Category = category,
                Currency = "USD",
                Price = 12.5m,
                Stock = 10,
                UnitToSell = "bag",
                PurchaseLeadTime = 2,
                PurchaseLeadTimeUnit = "day",
                Status = ProductStatus.Active
            },
            new Product
            {
                Id = 2,
                ProductName = "Tea",
                Description = "Green tea",
                CategoryId = 1,
                Category = category,
                Currency = "USD",
                Price = 7.25m,
                Stock = 4,
                UnitToSell = "box",
                PurchaseLeadTime = 1,
                PurchaseLeadTimeUnit = "day",
                Status = ProductStatus.Active
            },
            new Product
            {
                Id = 3,
                ProductCode = "SKU-003",
                ProductName = "Archived coffee",
                Description = "Do not expose",
                CategoryId = 1,
                Category = category,
                Currency = "USD",
                Price = 5m,
                Stock = 0,
                UnitToSell = "bag",
                Status = ProductStatus.Obsolete
            },
            new Product
            {
                Id = 4,
                ProductCode = "SKU-004",
                ProductName = "Untracked stock coffee",
                CategoryId = 1,
                Category = category,
                Currency = "USD",
                Price = 4m,
                Stock = null,
                UnitToSell = "bag",
                Status = ProductStatus.Active
            });

        await context.SaveChangesAsync();
    }
}
