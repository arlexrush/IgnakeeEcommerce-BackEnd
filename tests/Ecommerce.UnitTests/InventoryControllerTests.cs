using System.Net;
using System.Reflection;
using Ecommerce.Api.Controllers;
using Ecommerce.Application.Features.Inventory.Queries.GetInventoryProductByCode;
using Ecommerce.Application.Features.Inventory.Queries.PaginationInventoryProducts;
using Ecommerce.Application.Features.Inventory.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Models.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.UnitTests;

public class InventoryControllerTests
{
    [Fact]
    public void RequiresInventoryReaderOrAdminRole()
    {
        var attribute = typeof(InventoryController).GetCustomAttribute<AuthorizeAttribute>();

        Assert.NotNull(attribute);
        Assert.Equal($"{Role.ADMIN},{Role.INVENTORY_READER}", attribute!.Roles);
    }

    [Fact]
    public async Task GetInventoryProductByCodeRejectsBlankIdentifiers()
    {
        var controller = new InventoryController(new StubMediator());

        var result = await controller.GetInventoryProductByCode(" ");

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal((int)HttpStatusCode.BadRequest, badRequest.StatusCode);
    }

    [Theory]
    [InlineData(0, 10, null)]
    [InlineData(1, 0, null)]
    [InlineData(1, 10, 0)]
    public async Task PaginationInventoryProductsRejectsInvalidInput(int? pageIndex, int pageSize, int? categoryId)
    {
        var controller = new InventoryController(new StubMediator());

        var result = await controller.PaginationInventoryProducts(new PaginationInventoryProductsQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            CategoryId = categoryId
        });

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void PaginationInventoryProductsLimitsPageSizeToFifty()
    {
        var request = new PaginationInventoryProductsQuery
        {
            PageSize = 51
        };

        Assert.Equal(50, request.PageSize);
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
            object response = request switch
            {
                GetInventoryProductByCodeQuery => new InventoryProductVm(),
                PaginationInventoryProductsQuery => new PaginationVm<InventoryProductVm>(),
                _ => throw new InvalidOperationException("Unexpected request type.")
            };

            return Task.FromResult((TResponse)response);
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

        public async IAsyncEnumerable<TResponse> CreateStream<TResponse>(
            IStreamRequest<TResponse> request,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            yield break;
        }

        public async IAsyncEnumerable<object?> CreateStream(
            object request,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            yield break;
        }
    }
}
