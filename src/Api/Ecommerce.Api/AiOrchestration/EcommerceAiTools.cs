using System.ComponentModel;
using Ecommerce.Application.Features.Categories.Queries.GetCategoryList;
using Ecommerce.Application.Features.Countries.Queries.GetCountryList;
using Ecommerce.Application.Features.Countries.Queries.Vm;
using Ecommerce.Application.Features.Products.Queries.GetProductById;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Api.AiOrchestration;

public sealed class EcommerceAiTools
{
    private readonly IMediator _mediator;

    public EcommerceAiTools(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [Description("Obtiene los productos disponibles actualmente en el catálogo ecommerce.")]
    public Task<IReadOnlyList<Product>> GetProductCatalogAsync()
    {
        return _mediator.Send(new GetProductListQuery());
    }

    [Description("Obtiene un producto del catálogo a partir de su identificador numérico.")]
    public Task<ProductVm> GetProductAsync(
        [Description("El identificador numérico positivo del producto.")] int productId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId);
        return _mediator.Send(new GetProductByIdQuery(productId));
    }

    [Description("Obtiene las categorías disponibles en el catálogo ecommerce.")]
    public Task<IReadOnlyList<CategoryVm>> GetCategoriesAsync()
    {
        return _mediator.Send(new GetCategoryListQuery());
    }

    [Description("Obtiene los países configurados para checkout y envío.")]
    public Task<IReadOnlyList<CountryVm>> GetCountriesAsync()
    {
        return _mediator.Send(new GetCountryListQuery());
    }
}
