using System.ComponentModel;
using Ecommerce.Application.Features.Categories.Queries.GetCategoryList;
using Ecommerce.Application.Features.Countries.Queries.GetCountryList;
using Ecommerce.Application.Features.Countries.Queries.Vm;
using Ecommerce.Application.Features.Products.Queries.GetProductById;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Domain;
using MediatR;
using ModelContextProtocol.Server;

namespace Ecommerce.Api.McpServer;

/// <summary>
/// Es responsable for exposing the read-only tools of the ecommerce API to the Model Context Protocol (MCP) server.
/// </summary>
[McpServerToolType]
public sealed class EcommerceMcpTools
{
    private readonly IMediator _mediator;

    public EcommerceMcpTools(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [McpServerTool, Description("Returns the products currently available in the ecommerce catalog.")]
    public Task<IReadOnlyList<Product>> GetProductCatalogAsync()
    {
        return _mediator.Send(new GetProductListQuery());
    }

    [McpServerTool, Description("Returns a product from the catalog by its numeric identifier.")]
    public Task<ProductVm> GetProductAsync(
        [Description("The numeric identifier of the product.")] int productId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId);
        return _mediator.Send(new GetProductByIdQuery(productId));
    }

    [McpServerTool, Description("Returns all product categories available in the ecommerce catalog.")]
    public Task<IReadOnlyList<CategoryVm>> GetCategoriesAsync()
    {
        return _mediator.Send(new GetCategoryListQuery());
    }

    [McpServerTool, Description("Returns all countries configured for shipping and checkout.")]
    public Task<IReadOnlyList<CountryVm>> GetCountriesAsync()
    {
        return _mediator.Send(new GetCountryListQuery());
    }
}
