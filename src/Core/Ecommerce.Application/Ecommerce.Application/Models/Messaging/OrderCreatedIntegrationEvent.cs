using System.Text.Json.Serialization;

namespace Ecommerce.Application.Models.Messaging;

public sealed record OrderCreatedIntegrationEvent(
    int OrderId,
    string BuyerUserName,
    decimal Total,
    string Status,
    string? PaymentIntentId,
    IReadOnlyCollection<OrderCreatedItem> Items) : IIntegrationEvent
{
    [JsonIgnore]
    public string EventType => "orders.created";

    [JsonIgnore]
    public int ContractVersion => 1;
}

public sealed record OrderCreatedItem(
    int ProductId,
    string? ProductName,
    decimal Price,
    int Quantity);
