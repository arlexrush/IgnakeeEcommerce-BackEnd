using System.Text.Json;
using Ecommerce.Application.Models.Messaging;

namespace Ecommerce.UnitTests;

public class OrderCreatedIntegrationEventTests
{
    [Fact]
    public void SerializesVersionedEnvelopeWithExpectedContract()
    {
        var integrationEvent = new OrderCreatedIntegrationEvent(
            42,
            "buyer@example.com",
            125.50m,
            "Pending",
            "pi_123",
            [new OrderCreatedItem(7, "Product", 100m, 1)]);

        var envelope = new IntegrationEventEnvelope<OrderCreatedIntegrationEvent>(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            integrationEvent.EventType,
            integrationEvent.ContractVersion,
            new DateTimeOffset(2026, 1, 2, 3, 4, 5, TimeSpan.Zero),
            integrationEvent);
        var json = JsonSerializer.Serialize(envelope, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var deserialized = JsonSerializer.Deserialize<IntegrationEventEnvelope<OrderCreatedIntegrationEvent>>(
            json,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        Assert.Equal(envelope.MessageId, deserialized!.MessageId);
    }

    [Fact]
    public void PreservesOrderCreatedItemValues()
    {
        var item = new OrderCreatedItem(7, "Product", 100m, 2);

        Assert.Equal(new OrderCreatedItem(7, "Product", 100m, 2), item);
    }
}
