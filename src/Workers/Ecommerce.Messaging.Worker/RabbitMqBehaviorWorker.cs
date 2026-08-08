using System.Text;
using System.Text.Json;
using Ecommerce.Application.Models.Messaging;
using Ecommerce.Infrastructure.Messaging;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ecommerce.Messaging.Worker;

public sealed class RabbitMqBehaviorWorker : BackgroundService
{
    private const string RoutingKey = "behavior.recorded";
    private const string RetryCountHeader = "x-retry-count";
    private const string DeadLetterRoutingKey = "behavior.recorded.dead";
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly BehaviorWorkerOptions _workerOptions;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RabbitMqBehaviorWorker> _logger;
    private readonly SemaphoreSlim _channelGate = new(1, 1);

    public RabbitMqBehaviorWorker(
        IOptions<RabbitMqOptions> rabbitMqOptions,
        IOptions<BehaviorWorkerOptions> workerOptions,
        IServiceScopeFactory scopeFactory,
        ILogger<RabbitMqBehaviorWorker> logger)
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
                _logger.LogError(ex, "Behavior RabbitMQ consumer stopped unexpectedly; reconnecting.");
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
        consumer.Received += async (_, eventArgs) => await HandleDeliveryAsync(channel, eventArgs, stoppingToken);
        channel.BasicConsume(_workerOptions.QueueName, autoAck: false, consumer);

        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancellationRegistration = stoppingToken.Register(() => completion.TrySetResult());
        await completion.Task;
    }

    private async Task HandleDeliveryAsync(IModel channel, BasicDeliverEventArgs eventArgs, CancellationToken stoppingToken)
    {
        try
        {
            var envelope = JsonSerializer.Deserialize<IntegrationEventEnvelope<BehaviorRecordedIntegrationEvent>>(eventArgs.Body.Span, SerializerOptions)
                ?? throw new JsonException("The behavior event envelope is empty.");

            ValidateEnvelope(envelope);
            await ProcessIdempotentlyAsync(envelope, stoppingToken);
            await AcknowledgeAsync(channel, eventArgs.DeliveryTag, stoppingToken);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Behavior message {DeliveryTag} is invalid and will be dead-lettered.", eventArgs.DeliveryTag);
            await PublishDeadLetterAsync(channel, eventArgs, stoppingToken);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Behavior message {DeliveryTag} is incompatible and will be dead-lettered.", eventArgs.DeliveryTag);
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

    private async Task ProcessIdempotentlyAsync(IntegrationEventEnvelope<BehaviorRecordedIntegrationEvent> envelope, CancellationToken stoppingToken)
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(_workerOptions.ProcessingTimeoutSeconds));

        await using var scope = _scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EcommerceDbContext>();
        var messageId = envelope.MessageId.ToString();
        var alreadyProcessed = await dbContext.ProcessedBehaviorMessages
            .AsNoTracking()
            .AnyAsync(message => message.MessageId == messageId, timeout.Token);

        if (alreadyProcessed)
        {
            _logger.LogInformation("Skipping already processed behavior message {MessageId}.", messageId);
            return;
        }

        var handler = scope.ServiceProvider.GetRequiredService<BehaviorRecordedEventHandler>();
        await handler.HandleAsync(envelope.Payload, timeout.Token);
        dbContext.ProcessedBehaviorMessages.Add(new ProcessedBehaviorMessage
        {
            MessageId = messageId,
            EventType = envelope.EventType,
            ContractVersion = envelope.ContractVersion,
            ProcessedAtUtc = DateTimeOffset.UtcNow,
        });
        await dbContext.SaveChangesAsync(timeout.Token);
    }

    private static void ValidateEnvelope(IntegrationEventEnvelope<BehaviorRecordedIntegrationEvent> envelope)
    {
        if (!string.Equals(envelope.EventType, RoutingKey, StringComparison.Ordinal) ||
            envelope.ContractVersion != 1 ||
            !string.Equals(envelope.Payload.EventType, RoutingKey, StringComparison.Ordinal) ||
            envelope.Payload.ContractVersion != 1)
        {
            throw new InvalidOperationException($"Unsupported behavior event {envelope.EventType} version {envelope.ContractVersion}.");
        }
    }

    private async Task RetryOrDeadLetterAsync(IModel channel, BasicDeliverEventArgs eventArgs, Exception exception, CancellationToken stoppingToken)
    {
        var retryCount = GetRetryCount(eventArgs.BasicProperties);
        if (RetryPolicy.ShouldDeadLetter(retryCount, _workerOptions.MaxRetryAttempts))
        {
            _logger.LogError(exception, "Behavior message {DeliveryTag} exceeded retries and will be dead-lettered.", eventArgs.DeliveryTag);
            await PublishDeadLetterAsync(channel, eventArgs, stoppingToken);
            return;
        }

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = eventArgs.BasicProperties.ContentType;
        properties.Type = eventArgs.BasicProperties.Type;
        properties.Headers = new Dictionary<string, object> { [RetryCountHeader] = RetryPolicy.NextRetryAttempt(retryCount) };
        properties.Expiration = _workerOptions.RetryDelayMilliseconds.ToString();

        await _channelGate.WaitAsync(stoppingToken);
        try
        {
            channel.BasicPublish(string.Empty, _workerOptions.RetryQueueName, properties, eventArgs.Body);
            channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
        finally
        {
            _channelGate.Release();
        }
    }

    private async Task PublishDeadLetterAsync(IModel channel, BasicDeliverEventArgs eventArgs, CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = eventArgs.BasicProperties.ContentType;
        properties.Type = eventArgs.BasicProperties.Type;

        await _channelGate.WaitAsync(stoppingToken);
        try
        {
            channel.BasicPublish(_workerOptions.DeadLetterExchangeName, DeadLetterRoutingKey, properties, eventArgs.Body);
            channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
        finally
        {
            _channelGate.Release();
        }
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
            byte[] bytes when int.TryParse(Encoding.UTF8.GetString(bytes), out var count) => count,
            _ => 0,
        };
    }

    public override void Dispose()
    {
        _channelGate.Dispose();
        base.Dispose();
    }
}
