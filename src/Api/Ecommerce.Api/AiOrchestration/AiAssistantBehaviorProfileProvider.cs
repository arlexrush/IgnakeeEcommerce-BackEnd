using System.Security.Claims;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.AiOrchestration;

public sealed class AiAssistantBehaviorProfileProvider
{
    private readonly EcommerceDbContext _dbContext;

    public AiAssistantBehaviorProfileProvider(EcommerceDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<AiAssistantBehaviorProfile?> GetAsync(
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        var profile = await _dbContext.BehaviorProfiles
            .AsNoTracking()
            .Where(profile => profile.UserId == userId && profile.HasConsented)
            .Select(profile => new AiAssistantBehaviorProfile(
                profile.CatalogViews,
                profile.ProductViews,
                profile.CartAdditions,
                profile.CheckoutStarts,
                profile.LowestObservedProductPrice,
                profile.HighestObservedProductPrice,
                Array.Empty<string>(),
                Array.Empty<string>(),
                profile.LastActivityAtUtc))
            .SingleOrDefaultAsync(cancellationToken);

        if (profile is null)
        {
            return null;
        }

        var events = _dbContext.BehaviorEvents
            .AsNoTracking()
            .Where(behaviorEvent => behaviorEvent.UserId == userId);
        var preferredCategories = await events
            .Where(behaviorEvent => behaviorEvent.CategoryName != null)
            .GroupBy(behaviorEvent => behaviorEvent.CategoryName!)
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key)
            .Take(3)
            .Select(group => group.Key)
            .ToListAsync(cancellationToken);
        var recentProducts = await events
            .Where(behaviorEvent => behaviorEvent.ProductName != null)
            .OrderByDescending(behaviorEvent => behaviorEvent.OccurredOnUtc)
            .Select(behaviorEvent => behaviorEvent.ProductName!)
            .Distinct()
            .Take(5)
            .ToListAsync(cancellationToken);

        return profile with
        {
            PreferredCategories = preferredCategories,
            RecentProducts = recentProducts,
        };
    }
}
