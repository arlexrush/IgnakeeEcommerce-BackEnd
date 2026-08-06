using System.Text;
using System.Text.Json;
using Ecommerce.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Ecommerce.Infrastructure.Messaging;

public sealed class RabbitMqIntegrationEventPublisher : IIntegrationEventPublisher
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqIntegrationEventPublisher> _logger;

    public RabbitMqIntegrationEventPublisher(
        IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqIntegrationEventPublisher> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);
        _options = options.Value;
        _logger = logger;
    }

    public Task PublishAsync<TEvent>(
        TEvent integrationEvent,
        string routingKey,
        CancellationToken cancellationToken = default)
        where TEvent : class
    {
        ArgumentNullException.ThrowIfNull(integrationEvent);
        ArgumentException.ThrowIfNullOrWhiteSpace(routingKey);
        cancellationToken.ThrowIfCancellationRequested();

        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password,
            VirtualHost = _options.VirtualHost,
            DispatchConsumersAsync = true,
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.ExchangeDeclare(_options.ExchangeName, ExchangeType.Topic, durable: true, autoDelete: false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(integrationEvent, SerializerOptions));
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = "application/json";
        properties.Type = typeof(TEvent).Name;

        channel.BasicPublish(
            exchange: _options.ExchangeName,
            routingKey: routingKey,
            basicProperties: properties,
            body: body);

        _logger.LogDebug(
            "Published integration event {EventType} with routing key {RoutingKey} to exchange {ExchangeName}.",
            typeof(TEvent).Name,
            routingKey,
            _options.ExchangeName);

        return Task.CompletedTask;
    }
}
