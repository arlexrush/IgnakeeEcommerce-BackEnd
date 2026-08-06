namespace Ecommerce.Infrastructure.Messaging;

public sealed class ProcessedIntegrationMessage
{
    public required string MessageId { get; init; }
    public required string EventType { get; init; }
    public required int ContractVersion { get; init; }
    public DateTimeOffset ProcessedAtUtc { get; init; }
}
