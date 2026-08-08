namespace Ecommerce.Infrastructure.Messaging;

public sealed class ProcessedBehaviorMessage
{
    public required string MessageId { get; init; }
    public required string EventType { get; init; }
    public required int ContractVersion { get; init; }
    public required DateTimeOffset ProcessedAtUtc { get; init; }
}
