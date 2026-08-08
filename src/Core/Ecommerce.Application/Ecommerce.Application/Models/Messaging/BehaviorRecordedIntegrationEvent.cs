using System.Text.Json.Serialization;

namespace Ecommerce.Application.Models.Messaging;

public enum BehaviorAction
{
    CatalogViewed,
    ProductViewed,
    ProductAddedToCart,
    CheckoutStarted,
}

public sealed record BehaviorRecordedIntegrationEvent(
    string UserId,
    BehaviorAction Action,
    IReadOnlyCollection<int> ProductIds,
    int? CategoryId,
    DateTimeOffset OccurredOnUtc) : IIntegrationEvent
{
    [JsonIgnore]
    public string EventType => "behavior.recorded";

    [JsonIgnore]
    public int ContractVersion => 1;
}
