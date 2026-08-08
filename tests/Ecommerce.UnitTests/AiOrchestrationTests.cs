using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;
using Ecommerce.Api.AiOrchestration;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ecommerce.UnitTests;

public class AiOrchestrationTests
{
    [Fact]
    public void ExposesExpectedReadOnlyTools()
    {
        var methods = typeof(EcommerceAiTools)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        Assert.Equal(4, methods.Length);
        Assert.All(methods, method => Assert.NotNull(method.GetCustomAttribute<DescriptionAttribute>()));
    }

    [Fact]
    public async Task RejectsNonPositiveProductIdentifiers()
    {
        var tools = new EcommerceAiTools(new StubMediator());

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => tools.GetProductAsync(0));
    }

    [Fact]
    public async Task RejectsRequestsWhenFoundryIsNotConfigured()
    {
        var mediator = new StubMediator();
        var assistant = new FoundryAiAssistant(
            Options.Create(new AiOrchestrationOptions()),
            new EcommerceAiTools(mediator),
            new EcommerceAiPageContextProvider(mediator),
            new AiAssistantUserProfileProvider(),
            new AiAssistantBehaviorProfileProvider(CreateDbContext()));

        await Assert.ThrowsAsync<InvalidOperationException>(() => assistant.AskAsync(
            new AiAssistantRequest("Muestra el catálogo"),
            new ClaimsPrincipal(new ClaimsIdentity()),
            CancellationToken.None));
    }

    [Fact]
    public async Task RejectsProductDetailContextWithoutProductIdentifier()
    {
        var provider = new EcommerceAiPageContextProvider(new StubMediator());

        await Assert.ThrowsAsync<ArgumentException>(() => provider.GetAsync(
            new AiPageContext(AiPageContextKind.ProductDetail),
            CancellationToken.None));
    }

    [Fact]
    public void UsesTheAuthenticatedPublicName()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim("name", "Ana"),
            new Claim(ClaimTypes.Email, "ana@example.com")
        ],
        "test"));

        var profile = new AiAssistantUserProfileProvider().Get(user);

        Assert.Equal("Ana", profile.DisplayName);
    }

    [Fact]
    public void DoesNotUseEmailAsPublicName()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [new Claim("preferred_username", "ana@example.com")],
        "test"));

        var profile = new AiAssistantUserProfileProvider().Get(user);

        Assert.Null(profile.DisplayName);
    }

    [Fact]
    public async Task DoesNotLoadBehaviorProfileWithoutAuthenticatedIdentifier()
    {
        var provider = new AiAssistantBehaviorProfileProvider(CreateDbContext());

        var profile = await provider.GetAsync(new ClaimsPrincipal(new ClaimsIdentity()), CancellationToken.None);

        Assert.Null(profile);
    }

    [Fact]
    public async Task SummarizesConsentedBehaviorPreferences()
    {
        await using var dbContext = CreateDbContext();
        dbContext.BehaviorProfiles.Add(new Ecommerce.Infrastructure.Messaging.BehaviorProfile
        {
            UserId = "user-123",
            HasConsented = true,
            LowestObservedProductPrice = 10m,
            HighestObservedProductPrice = 50m,
        });
        dbContext.BehaviorEvents.AddRange(
            new Ecommerce.Infrastructure.Messaging.BehaviorEvent
            {
                UserId = "user-123",
                Action = Ecommerce.Application.Models.Messaging.BehaviorAction.ProductViewed,
                ProductName = "Café",
                CategoryName = "Alimentación",
                OccurredOnUtc = DateTimeOffset.UtcNow,
            },
            new Ecommerce.Infrastructure.Messaging.BehaviorEvent
            {
                UserId = "user-123",
                Action = Ecommerce.Application.Models.Messaging.BehaviorAction.ProductAddedToCart,
                ProductName = "Té",
                CategoryName = "Alimentación",
                OccurredOnUtc = DateTimeOffset.UtcNow.AddMinutes(-1),
            });
        await dbContext.SaveChangesAsync();
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "user-123")], "test"));

        var profile = await new AiAssistantBehaviorProfileProvider(dbContext).GetAsync(user, CancellationToken.None);

        Assert.Equal("Alimentación", Assert.Single(profile!.PreferredCategories));
    }

    private static Ecommerce.Infrastructure.Persistence.EcommerceDbContext CreateDbContext()
    {
        var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<Ecommerce.Infrastructure.Persistence.EcommerceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new Ecommerce.Infrastructure.Persistence.EcommerceDbContext(options);
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

        private static async IAsyncEnumerable<T> Empty<T>()
        {
            await Task.CompletedTask;
            yield break;
        }
    }
}
