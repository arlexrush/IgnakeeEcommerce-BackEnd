namespace Ecommerce.Messaging.Worker;

public sealed class WorkerOptions
{
    public const string SectionName = "MessagingWorker";

    public string QueueName { get; set; } = "ecommerce.orders.created";
    public string RetryQueueName { get; set; } = "ecommerce.orders.created.retry";
    public string DeadLetterExchangeName { get; set; } = "ecommerce.integration.dlx";
    public string DeadLetterQueueName { get; set; } = "ecommerce.orders.created.dlq";
    public ushort PrefetchCount { get; set; } = 4;
    public int MaxRetryAttempts { get; set; } = 3;
    public int RetryDelayMilliseconds { get; set; } = 5000;
    public int ProcessingTimeoutSeconds { get; set; } = 30;
}
