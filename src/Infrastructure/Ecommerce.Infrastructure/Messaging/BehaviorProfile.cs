namespace Ecommerce.Infrastructure.Messaging;

public sealed class BehaviorProfile
{
    public required string UserId { get; init; }
    public bool HasConsented { get; set; }
    public DateTimeOffset? ConsentUpdatedAtUtc { get; set; }
    public int CatalogViews { get; set; }
    public int ProductViews { get; set; }
    public int CartAdditions { get; set; }
    public int CheckoutStarts { get; set; }
    public decimal? LowestObservedProductPrice { get; set; }
    public decimal? HighestObservedProductPrice { get; set; }
    public DateTimeOffset? LastActivityAtUtc { get; set; }
}
