namespace Ecommerce.Api.AiOrchestration;

public sealed record AiAssistantBehaviorProfile(
    int CatalogViews,
    int ProductViews,
    int CartAdditions,
    int CheckoutStarts,
    decimal? LowestObservedProductPrice,
    decimal? HighestObservedProductPrice,
    IReadOnlyCollection<string> PreferredCategories,
    IReadOnlyCollection<string> RecentProducts,
    DateTimeOffset? LastActivityAtUtc);
