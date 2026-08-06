using Ecommerce.Application.Models.Messaging;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Messaging.Worker;

public sealed class OrderCreatedEventHandler
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(OrderCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogInformation(
            "Processed OrderCreatedIntegrationEvent for order {OrderId} and buyer {BuyerUserName}.",
            integrationEvent.OrderId,
            integrationEvent.BuyerUserName);

        return Task.CompletedTask;
    }
}
