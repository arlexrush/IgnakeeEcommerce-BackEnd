namespace Ecommerce.Messaging.Worker;

public sealed class BehaviorWorkerOptions
{
    public const string SectionName = "BehaviorMessagingWorker";

    public string QueueName { get; set; } = "ecommerce.behavior.recorded";
    public string RetryQueueName { get; set; } = "ecommerce.behavior.recorded.retry";
    public string DeadLetterExchangeName { get; set; } = "ecommerce.behavior.dlx";
    public string DeadLetterQueueName { get; set; } = "ecommerce.behavior.recorded.dlq";
    public ushort PrefetchCount { get; set; } = 8;
    public int MaxRetryAttempts { get; set; } = 3;
    public int RetryDelayMilliseconds { get; set; } = 5000;
    public int ProcessingTimeoutSeconds { get; set; } = 30;
}
