using System.Text.Json;
using Ecommerce.Application.Models.Messaging;

namespace Ecommerce.UnitTests;

public class BehaviorRecordedIntegrationEventTests
{
    [Fact]
    public void ExposesExpectedVersionedContract()
    {
        var integrationEvent = new BehaviorRecordedIntegrationEvent(
            "user-123",
            BehaviorAction.ProductViewed,
            [42],
            7,
            new DateTimeOffset(2026, 8, 8, 12, 0, 0, TimeSpan.Zero));

        Assert.Equal("behavior.recorded", integrationEvent.EventType);
    }

    [Fact]
    public void SerializesVersionedEnvelopeWithExpectedPayload()
    {
        var integrationEvent = new BehaviorRecordedIntegrationEvent(
            "user-123",
            BehaviorAction.ProductAddedToCart,
            [42, 43],
            null,
            new DateTimeOffset(2026, 8, 8, 12, 0, 0, TimeSpan.Zero));
        var envelope = new IntegrationEventEnvelope<BehaviorRecordedIntegrationEvent>(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            integrationEvent.EventType,
            integrationEvent.ContractVersion,
            integrationEvent.OccurredOnUtc,
            integrationEvent);

        var json = JsonSerializer.Serialize(envelope, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var deserialized = JsonSerializer.Deserialize<IntegrationEventEnvelope<BehaviorRecordedIntegrationEvent>>(
            json,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        Assert.Equal(BehaviorAction.ProductAddedToCart, deserialized!.Payload.Action);
        Assert.Equal([42, 43], deserialized.Payload.ProductIds);
    }
}
