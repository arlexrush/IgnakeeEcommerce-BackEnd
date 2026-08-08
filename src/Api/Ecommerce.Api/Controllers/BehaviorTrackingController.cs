using System.Security.Claims;
using Ecommerce.Api.BehaviorTracking;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Messaging;
using Ecommerce.Infrastructure.Messaging;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/behavior")]
public sealed class BehaviorTrackingController : ControllerBase
{
    private const string RoutingKey = "behavior.recorded";
    private readonly EcommerceDbContext _dbContext;
    private readonly IIntegrationEventPublisher _publisher;

    public BehaviorTrackingController(EcommerceDbContext dbContext, IIntegrationEventPublisher publisher)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(publisher);
        _dbContext = dbContext;
        _publisher = publisher;
    }

    [HttpPut("consent")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> SetConsentAsync(BehaviorConsentRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var profile = await _dbContext.BehaviorProfiles.FindAsync([userId], cancellationToken);

        if (!request.Granted)
        {
            await _dbContext.BehaviorEvents
                .Where(behaviorEvent => behaviorEvent.UserId == userId)
                .ExecuteDeleteAsync(cancellationToken);

            if (profile is not null)
            {
                _dbContext.BehaviorProfiles.Remove(profile);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return NoContent();
        }

        if (profile is null)
        {
            profile = new BehaviorProfile { UserId = userId };
            _dbContext.BehaviorProfiles.Add(profile);
        }

        profile.HasConsented = true;
        profile.ConsentUpdatedAtUtc = DateTimeOffset.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPost("events")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> TrackAsync(BehaviorTrackingRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var hasConsent = await _dbContext.BehaviorProfiles
            .AsNoTracking()
            .AnyAsync(profile => profile.UserId == userId && profile.HasConsented, cancellationToken);

        if (!hasConsent)
        {
            return Forbid();
        }

        var behaviorEvent = new BehaviorRecordedIntegrationEvent(
            userId,
            request.Action!.Value,
            request.ProductIds.Where(productId => productId > 0).Distinct().ToArray(),
            request.CategoryId,
            DateTimeOffset.UtcNow);
        await _publisher.PublishAsync(behaviorEvent, RoutingKey, cancellationToken);
        return Accepted();
    }

    private string GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new InvalidOperationException("The authenticated user identifier is unavailable.");
        }

        return userId;
    }
}
