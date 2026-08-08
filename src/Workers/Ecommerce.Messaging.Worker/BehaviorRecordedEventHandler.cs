using Ecommerce.Application.Models.Messaging;
using Ecommerce.Infrastructure.Messaging;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Messaging.Worker;

public sealed class BehaviorRecordedEventHandler
{
    private readonly EcommerceDbContext _dbContext;

    public BehaviorRecordedEventHandler(EcommerceDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task HandleAsync(BehaviorRecordedIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        var profile = await _dbContext.BehaviorProfiles.FindAsync([integrationEvent.UserId], cancellationToken)
            ?? throw new InvalidOperationException("Behavior consent is not registered for the event user.");

        if (!profile.HasConsented)
        {
            return;
        }

        switch (integrationEvent.Action)
        {
            case BehaviorAction.CatalogViewed:
                profile.CatalogViews++;
                break;
            case BehaviorAction.ProductViewed:
                profile.ProductViews++;
                break;
            case BehaviorAction.ProductAddedToCart:
                profile.CartAdditions++;
                break;
            case BehaviorAction.CheckoutStarted:
                profile.CheckoutStarts++;
                break;
            default:
                throw new InvalidOperationException($"Unsupported behavior action {integrationEvent.Action}.");
        }

        var products = await _dbContext.Products!
            .AsNoTracking()
            .Include(product => product.Category)
            .Where(product => product.Id.HasValue && integrationEvent.ProductIds.Contains(product.Id.Value))
            .ToListAsync(cancellationToken);

        foreach (var product in products)
        {
            _dbContext.BehaviorEvents.Add(new BehaviorEvent
            {
                UserId = integrationEvent.UserId,
                Action = integrationEvent.Action,
                ProductId = product.Id,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                ProductPrice = product.Price,
                OccurredOnUtc = integrationEvent.OccurredOnUtc,
            });

            if (product.Price is { } price)
            {
                profile.LowestObservedProductPrice = profile.LowestObservedProductPrice is { } lowest
                    ? Math.Min(lowest, price)
                    : price;
                profile.HighestObservedProductPrice = profile.HighestObservedProductPrice is { } highest
                    ? Math.Max(highest, price)
                    : price;
            }
        }

        profile.LastActivityAtUtc = integrationEvent.OccurredOnUtc;
    }
}
