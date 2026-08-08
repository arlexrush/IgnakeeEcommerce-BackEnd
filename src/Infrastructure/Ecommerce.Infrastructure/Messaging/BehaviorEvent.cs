using Ecommerce.Application.Models.Messaging;

namespace Ecommerce.Infrastructure.Messaging;

public sealed class BehaviorEvent
{
    public long Id { get; init; }
    public required string UserId { get; init; }
    public required BehaviorAction Action { get; init; }
    public int? ProductId { get; init; }
    public string? ProductName { get; init; }
    public int? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public decimal? ProductPrice { get; init; }
    public required DateTimeOffset OccurredOnUtc { get; init; }
}
