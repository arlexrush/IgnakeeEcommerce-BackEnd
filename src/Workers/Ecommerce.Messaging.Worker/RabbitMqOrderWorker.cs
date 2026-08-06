using System.Text.Json;
using Ecommerce.Application.Models.Messaging;
using Ecommerce.Infrastructure.Messaging;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ecommerce.Messaging.Worker;

public sealed class RabbitMqOrderWorker : BackgroundService
{
    private const string RoutingKey = "orders.created";
    private const string RetryCountHeader = "x-retry-count";
    private const string DeadLetterRoutingKey = "orders.created.dead";
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly WorkerOptions _workerOptions;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RabbitMqOrderWorker> _logger;
    private readonly SemaphoreSlim _channelGate = new(1, 1);

    public RabbitMqOrderWorker(
        IOptions<RabbitMqOptions> rabbitMqOptions,
        IOptions<WorkerOptions> workerOptions,
        IServiceScopeFactory scopeFactory,
        ILogger<RabbitMqOrderWorker> logger)
    {
        _rabbitMqOptions = rabbitMqOptions.Value;
        _workerOptions = workerOptions.Value;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunConsumerAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RabbitMQ consumer stopped unexpectedly; reconnecting.");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task RunConsumerAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqOptions.HostName,
            Port = _rabbitMqOptions.Port,
            UserName = _rabbitMqOptions.UserName,
            Password = _rabbitMqOptions.Password,
            VirtualHost = _rabbitMqOptions.VirtualHost,
            DispatchConsumersAsync = true,
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        DeclareTopology(channel);
        channel.BasicQos(0, _workerOptions.PrefetchCount, global: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (_, eventArgs) =>
        {
            await HandleDeliveryAsync(channel, eventArgs, stoppingToken);
        };

        channel.BasicConsume(_workerOptions.QueueName, autoAck: false, consumer);

        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancellationRegistration = stoppingToken.Register(() => completion.TrySetResult());
        await completion.Task;
    }

    private async Task HandleDeliveryAsync(
        IModel channel,
        BasicDeliverEventArgs eventArgs,
        CancellationToken stoppingToken)
    {
        try
        {
            var envelope = JsonSerializer.Deserialize<IntegrationEventEnvelope<OrderCreatedIntegrationEvent>>(
                eventArgs.Body.Span,
                SerializerOptions)
                ?? throw new JsonException("The integration event envelope is empty.");

            ValidateEnvelope(envelope);
            await ProcessIdempotentlyAsync(envelope, stoppingToken);
            await AcknowledgeAsync(channel, eventArgs.DeliveryTag, stoppingToken);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Message {DeliveryTag} is invalid and will be dead-lettered.", eventArgs.DeliveryTag);
            await PublishDeadLetterAsync(channel, eventArgs, stoppingToken);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Message {DeliveryTag} is incompatible and will be dead-lettered.", eventArgs.DeliveryTag);
            await PublishDeadLetterAsync(channel, eventArgs, stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            await RetryOrDeadLetterAsync(channel, eventArgs, ex, stoppingToken);
        }
    }

    private async Task ProcessIdempotentlyAsync(
        IntegrationEventEnvelope<OrderCreatedIntegrationEvent> envelope,
        CancellationToken stoppingToken)
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(_workerOptions.ProcessingTimeoutSeconds));

        await using var scope = _scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EcommerceDbContext>();
        var messageId = envelope.MessageId.ToString();
        var alreadyProcessed = await dbContext.ProcessedIntegrationMessages
            .AsNoTracking()
            .AnyAsync(message => message.MessageId == messageId, timeout.Token);

        if (alreadyProcessed)
        {
            _logger.LogInformation("Skipping already processed integration message {MessageId}.", messageId);
            return;
        }

        var handler = scope.ServiceProvider.GetRequiredService<OrderCreatedEventHandler>();
        await handler.HandleAsync(envelope.Payload, timeout.Token);

        dbContext.ProcessedIntegrationMessages.Add(new ProcessedIntegrationMessage
        {
            MessageId = messageId,
            EventType = envelope.EventType,
            ContractVersion = envelope.ContractVersion,
            ProcessedAtUtc = DateTimeOffset.UtcNow,
        });
        await dbContext.SaveChangesAsync(timeout.Token);
    }

    private void ValidateEnvelope(IntegrationEventEnvelope<OrderCreatedIntegrationEvent> envelope)
    {
        if (!string.Equals(envelope.EventType, RoutingKey, StringComparison.Ordinal) ||
            envelope.ContractVersion != 1 ||
            !string.Equals(envelope.Payload.EventType, RoutingKey, StringComparison.Ordinal) ||
            envelope.Payload.ContractVersion != 1)
        {
            throw new InvalidOperationException(
                $"Unsupported event {envelope.EventType} version {envelope.ContractVersion}.");
        }
    }

    private async Task RetryOrDeadLetterAsync(
        IModel channel,
        BasicDeliverEventArgs eventArgs,
        Exception exception,
        CancellationToken stoppingToken)
    {
        var retryCount = GetRetryCount(eventArgs.BasicProperties);
        if (RetryPolicy.ShouldDeadLetter(retryCount, _workerOptions.MaxRetryAttempts))
        {
            _logger.LogError(
                exception,
                "Message {DeliveryTag} exceeded {MaxRetryAttempts} retry attempts and will be dead-lettered.",
                eventArgs.DeliveryTag,
                _workerOptions.MaxRetryAttempts);
            await PublishDeadLetterAsync(channel, eventArgs, stoppingToken);
            return;
        }

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = eventArgs.BasicProperties.ContentType;
        properties.Type = eventArgs.BasicProperties.Type;
        properties.Headers = new Dictionary<string, object>
        {
            [RetryCountHeader] = RetryPolicy.NextRetryAttempt(retryCount),
        };
        properties.Expiration = _workerOptions.RetryDelayMilliseconds.ToString();

        await _channelGate.WaitAsync(stoppingToken);
        try
        {
            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _workerOptions.RetryQueueName,
                basicProperties: properties,
                body: eventArgs.Body);
            channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
        finally
        {
            _channelGate.Release();
        }
        _logger.LogWarning(
            exception,
            "Message {DeliveryTag} scheduled for retry {RetryAttempt}.",
            eventArgs.DeliveryTag,
            RetryPolicy.NextRetryAttempt(retryCount));
    }

    private async Task PublishDeadLetterAsync(
        IModel channel,
        BasicDeliverEventArgs eventArgs,
        CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = eventArgs.BasicProperties.ContentType;
        properties.Type = eventArgs.BasicProperties.Type;
        await _channelGate.WaitAsync(stoppingToken);
        try
        {
            channel.BasicPublish(
                _workerOptions.DeadLetterExchangeName,
                DeadLetterRoutingKey,
                properties,
                eventArgs.Body);
            channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
        finally
        {
            _channelGate.Release();
        }
        await Task.CompletedTask;
    }

    private async Task AcknowledgeAsync(IModel channel, ulong deliveryTag, CancellationToken stoppingToken)
    {
        await _channelGate.WaitAsync(stoppingToken);
        try
        {
            channel.BasicAck(deliveryTag, multiple: false);
        }
        finally
        {
            _channelGate.Release();
        }
    }

    private void DeclareTopology(IModel channel)
    {
        channel.ExchangeDeclare(_rabbitMqOptions.ExchangeName, ExchangeType.Topic, durable: true, autoDelete: false);
        channel.ExchangeDeclare(_workerOptions.DeadLetterExchangeName, ExchangeType.Direct, durable: true, autoDelete: false);

        channel.QueueDeclare(_workerOptions.QueueName, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(_workerOptions.QueueName, _rabbitMqOptions.ExchangeName, RoutingKey);

        var retryArguments = new Dictionary<string, object>
        {
            ["x-message-ttl"] = _workerOptions.RetryDelayMilliseconds,
            ["x-dead-letter-exchange"] = _rabbitMqOptions.ExchangeName,
            ["x-dead-letter-routing-key"] = RoutingKey,
        };
        channel.QueueDeclare(_workerOptions.RetryQueueName, durable: true, exclusive: false, autoDelete: false, arguments: retryArguments);

        channel.QueueDeclare(_workerOptions.DeadLetterQueueName, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(_workerOptions.DeadLetterQueueName, _workerOptions.DeadLetterExchangeName, DeadLetterRoutingKey);
    }

    private static int GetRetryCount(IBasicProperties properties)
    {
        if (properties.Headers is null || !properties.Headers.TryGetValue(RetryCountHeader, out var value))
        {
            return 0;
        }

        return value switch
        {
            int count => count,
            long count => checked((int)count),
            byte[] bytes when int.TryParse(System.Text.Encoding.UTF8.GetString(bytes), out var count) => count,
            _ => 0,
        };
    }

    public override void Dispose()
    {
        _channelGate.Dispose();
        base.Dispose();
    }
}
