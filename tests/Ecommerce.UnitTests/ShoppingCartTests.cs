using Ecommerce.Domain;

namespace Ecommerce.UnitTests;

public class ShoppingCartTests
{
    [Fact]
    public void AddItemAddsItemAndAssociatesCart()
    {
        var cart = new ShoppingCart { Id = 10, ShoppingCartMasterId = Guid.NewGuid() };
        var item = CreateItem(3, 2, 4.50m);

        cart.AddItem(item);

        Assert.Same(item, Assert.Single(cart.ShoppingCartItems!));
        Assert.Equal(10, item.ShoppingCartId);
        Assert.Equal(cart.ShoppingCartMasterId, item.ShoppingCartMasterId);
    }

    [Fact]
    public void AddItemMergesItemsForTheSameProduct()
    {
        var cart = new ShoppingCart();
        cart.AddItem(CreateItem(3, 2, 4.50m));

        cart.AddItem(CreateItem(3, 1, 5m));

        var item = Assert.Single(cart.ShoppingCartItems!);
        Assert.Equal(3, item.Quantity);
        Assert.Equal(5m, item.Price);
    }

    [Fact]
    public void AddItemRejectsNonPositiveQuantity()
    {
        var cart = new ShoppingCart();

        Assert.Throws<ArgumentOutOfRangeException>(() => cart.AddItem(CreateItem(3, 0, 4.50m)));
    }

    [Fact]
    public void UpdateItemQuantityChangesExistingItem()
    {
        var cart = new ShoppingCart();
        cart.AddItem(CreateItem(3, 1, 4.50m));

        cart.UpdateItemQuantity(3, 4);

        Assert.Equal(4, Assert.Single(cart.ShoppingCartItems!).Quantity);
    }

    [Fact]
    public void UpdateItemQuantityZeroRemovesItem()
    {
        var cart = new ShoppingCart();
        cart.AddItem(CreateItem(3, 1, 4.50m));

        cart.UpdateItemQuantity(3, 0);

        Assert.Empty(cart.ShoppingCartItems!);
    }

    [Fact]
    public void UpdateItemQuantityRejectsUnknownProduct()
    {
        var cart = new ShoppingCart();

        Assert.Throws<InvalidOperationException>(() => cart.UpdateItemQuantity(3, 1));
    }

    [Fact]
    public void GetSubtotalSumsAllItemLines()
    {
        var cart = new ShoppingCart();
        cart.AddItem(CreateItem(1, 2, 4.50m));
        cart.AddItem(CreateItem(2, 1, 3m));

        var subtotal = cart.GetSubtotal();

        Assert.Equal(12m, subtotal);
    }

    private static ShoppingCartItem CreateItem(int productId, int quantity, decimal price)
    {
        return new ShoppingCartItem { ProductId = productId, Quantity = quantity, Price = price };
    }
}
