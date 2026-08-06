namespace Ecommerce.Application.Contracts.Infrastructure;

public interface IIntegrationEventPublisher
{
    Task PublishAsync<TEvent>(TEvent integrationEvent, string routingKey, CancellationToken cancellationToken = default)
        where TEvent : class;
}
