using Ecommerce.Messaging.Worker;

namespace Ecommerce.UnitTests;

public class RetryPolicyTests
{
    [Fact]
    public void AllowsRetryBeforeMaximumAttempt()
    {
        var shouldDeadLetter = RetryPolicy.ShouldDeadLetter(2, 3);

        Assert.False(shouldDeadLetter);
    }

    [Fact]
    public void DeadLettersAtMaximumAttempt()
    {
        var shouldDeadLetter = RetryPolicy.ShouldDeadLetter(3, 3);

        Assert.True(shouldDeadLetter);
    }

    [Fact]
    public void AdvancesRetryAttempt()
    {
        var nextAttempt = RetryPolicy.NextRetryAttempt(2);

        Assert.Equal(3, nextAttempt);
    }
}
