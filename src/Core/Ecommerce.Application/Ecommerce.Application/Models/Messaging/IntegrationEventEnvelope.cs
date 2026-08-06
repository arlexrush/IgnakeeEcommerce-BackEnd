using System.Text.Json.Serialization;

namespace Ecommerce.Application.Models.Messaging;

public sealed record IntegrationEventEnvelope<TEvent>
    where TEvent : IIntegrationEvent
{
    public Guid MessageId { get; init; }
    public string EventType { get; init; } = string.Empty;
    public int ContractVersion { get; init; }
    public DateTimeOffset OccurredOnUtc { get; init; }
    public TEvent Payload { get; init; } = default!;

    public IntegrationEventEnvelope()
    {
    }

    [JsonConstructor]
    public IntegrationEventEnvelope(
        Guid messageId,
        string eventType,
        int contractVersion,
        DateTimeOffset occurredOnUtc,
        TEvent payload)
    {
        MessageId = messageId;
        EventType = eventType;
        ContractVersion = contractVersion;
        OccurredOnUtc = occurredOnUtc;
        Payload = payload;
    }
}
