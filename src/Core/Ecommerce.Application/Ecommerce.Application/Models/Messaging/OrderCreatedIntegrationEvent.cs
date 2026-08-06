namespace Ecommerce.Application.Models.Messaging;

public sealed record OrderCreatedIntegrationEvent(
    int OrderId,
    string BuyerUserName,
    decimal Total,
    string Status,
    string? PaymentIntentId,
    IReadOnlyCollection<OrderCreatedItem> Items);

public sealed record OrderCreatedItem(
    int ProductId,
    string? ProductName,
    decimal Price,
    int Quantity);
