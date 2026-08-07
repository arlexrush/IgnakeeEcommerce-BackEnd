using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;
using Ecommerce.Api.McpServer;
using MediatR;
using ModelContextProtocol.Server;

namespace Ecommerce.UnitTests;

public class EcommerceMcpToolsTests
{
    [Fact]
    public void ExposesExpectedReadOnlyTools()
    {
        var toolType = typeof(EcommerceMcpTools);
        var toolTypeAttribute = toolType.GetCustomAttribute<McpServerToolTypeAttribute>();
        var methods = toolType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(method => method.GetCustomAttribute<McpServerToolAttribute>() is not null)
            .ToArray();

        Assert.NotNull(toolTypeAttribute);
        Assert.Equal(4, methods.Length);
        Assert.All(methods, method => Assert.NotNull(method.GetCustomAttribute<DescriptionAttribute>()));
    }

    [Fact]
    public async Task RejectsNonPositiveProductIdentifiers()
    {
        var tools = new EcommerceMcpTools(new StubMediator());

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => tools.GetProductAsync(0));
    }

    private sealed class StubMediator : IMediator
    {
        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
        {
            return Task.CompletedTask;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<TResponse>(default!);
        }

        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<object?>(null);
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            return Task.CompletedTask;
        }

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(
            IStreamRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            return Empty<TResponse>();
        }

        public IAsyncEnumerable<object?> CreateStream(
            object request,
            CancellationToken cancellationToken = default)
        {
            return Empty<object?>();
        }

        private static async IAsyncEnumerable<T> Empty<T>(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            yield break;
        }
    }
}
