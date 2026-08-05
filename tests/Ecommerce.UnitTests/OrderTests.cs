using Ecommerce.Domain;

namespace Ecommerce.UnitTests;

public class OrderTests
{
    [Fact]
    public void ApplyPricingCalculatesTotal()
    {
        var order = new Order();

        order.ApplyPricing(100m, 21m, 5m);

        Assert.Equal(126m, order.Total);
    }

    [Fact]
    public void ApplyPricingRejectsNegativeSubtotal()
    {
        var order = new Order();

        Assert.Throws<ArgumentOutOfRangeException>(() => order.ApplyPricing(-1m, 0m, 0m));
    }

    [Fact]
    public void AddItemMergesSameProductAndUpdatesLineData()
    {
        var order = new Order { Id = 20 };
        order.AddItem(CreateItem(4, 2, 10m, "Coffee"));

        order.AddItem(CreateItem(4, 1, 12m, "Premium coffee"));

        var item = Assert.Single(order.OrderItems!);
        Assert.Equal(3, item.Quantity);
        Assert.Equal(12m, item.Price);
        Assert.Equal("Premium coffee", item.productName);
    }

    [Fact]
    public void AddItemAssociatesNewItemWithOrder()
    {
        var order = new Order { Id = 20 };
        var item = CreateItem(4, 1, 10m, "Coffee");

        order.AddItem(item);

        Assert.Equal(20, item.OrderId);
    }

    [Fact]
    public void AddItemRejectsNonPositiveQuantity()
    {
        var order = new Order();

        Assert.Throws<ArgumentOutOfRangeException>(() => order.AddItem(CreateItem(4, 0, 10m, "Coffee")));
    }

    [Fact]
    public void GetLineTotalCalculatesQuantityTimesPrice()
    {
        var item = CreateItem(4, 3, 10m, "Coffee");

        var lineTotal = item.GetLineTotal();

        Assert.Equal(30m, lineTotal);
    }

    [Fact]
    public void GetLineTotalRejectsInvalidQuantity()
    {
        var item = CreateItem(4, 0, 10m, "Coffee");

        Assert.Throws<ArgumentOutOfRangeException>(() => item.GetLineTotal());
    }

    [Fact]
    public void SetShippingDetailsRejectsNegativeWeight()
    {
        var order = new Order();

        Assert.Throws<ArgumentOutOfRangeException>(() => order.SetShippingDetails("Carrier", 5m, -1));
    }

    [Fact]
    public void MarkAsApprovedChangesStatus()
    {
        var order = new Order();

        order.MarkAsApproved();

        Assert.Equal(OrderStatus.Approved, order.orderStatus);
    }

    [Fact]
    public void SetPaymentDetailsStoresPaymentInformation()
    {
        var order = new Order();

        order.SetPaymentDetails("pi_123", "secret", "key");

        Assert.Equal("pi_123", order.PaymentIntentId);
        Assert.Equal("secret", order.ClientSecret);
        Assert.Equal("key", order.StripeApiKey);
    }

    [Fact]
    public void MarkPaymentSucceededApprovesOrder()
    {
        var order = new Order();

        order.MarkPaymentSucceeded();

        Assert.Equal(PaymentStatus.Succeeded, order.PaymentStatus);
        Assert.Equal(OrderStatus.Approved, order.orderStatus);
    }

    [Fact]
    public void MarkPaymentFailedSetsErrorStatus()
    {
        var order = new Order();

        order.MarkPaymentFailed();

        Assert.Equal(PaymentStatus.Failed, order.PaymentStatus);
        Assert.Equal(OrderStatus.Error, order.orderStatus);
    }

    [Fact]
    public void MarkPaymentProcessingKeepsOrderPending()
    {
        var order = new Order();

        order.MarkPaymentProcessing();

        Assert.Equal(PaymentStatus.Processing, order.PaymentStatus);
        Assert.Equal(OrderStatus.Pending, order.orderStatus);
    }

    private static OrderItem CreateItem(int productId, int quantity, decimal price, string productName)
    {
        return new OrderItem
        {
            ProductId = productId,
            Quantity = quantity,
            Price = price,
            productName = productName
        };
    }
}
