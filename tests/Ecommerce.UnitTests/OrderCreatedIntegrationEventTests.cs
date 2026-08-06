using System.Text.Json;
using Ecommerce.Application.Models.Messaging;

namespace Ecommerce.UnitTests;

public class OrderCreatedIntegrationEventTests
{
    [Fact]
    public void SerializesOrderCreatedEventWithExpectedContract()
    {
        var integrationEvent = new OrderCreatedIntegrationEvent(
            42,
            "buyer@example.com",
            125.50m,
            "Pending",
            "pi_123",
            [new OrderCreatedItem(7, "Product", 100m, 1)]);

        var json = JsonSerializer.Serialize(integrationEvent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        Assert.Equal(
            "{\"orderId\":42,\"buyerUserName\":\"buyer@example.com\",\"total\":125.50,\"status\":\"Pending\",\"paymentIntentId\":\"pi_123\",\"items\":[{\"productId\":7,\"productName\":\"Product\",\"price\":100,\"quantity\":1}]}",
            json);
    }

    [Fact]
    public void PreservesOrderCreatedItemValues()
    {
        var item = new OrderCreatedItem(7, "Product", 100m, 2);

        Assert.Equal(new OrderCreatedItem(7, "Product", 100m, 2), item);
    }
}
