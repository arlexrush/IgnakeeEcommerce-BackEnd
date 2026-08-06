namespace Ecommerce.Messaging.Worker;

public static class RetryPolicy
{
    public static bool ShouldDeadLetter(int retryCount, int maxRetryAttempts)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(retryCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxRetryAttempts);
        return retryCount >= maxRetryAttempts;
    }

    public static int NextRetryAttempt(int retryCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(retryCount);
        return checked(retryCount + 1);
    }
}
