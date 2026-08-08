using Ecommerce.Application.Features.Products.Queries.GetProductById;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Features.Products.Queries.PaginationProducts;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Api.AiOrchestration;

public sealed class EcommerceAiPageContextProvider
{
    private const int CatalogOverviewLimit = 12;
    private const int SimilarProductsLimit = 4;
    private readonly IMediator _mediator;

    public EcommerceAiPageContextProvider(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    public async Task<AiPageData> GetAsync(AiPageContext? pageContext, CancellationToken cancellationToken)
    {
        return pageContext?.Kind switch
        {
            AiPageContextKind.ProductDetail => await GetProductDetailAsync(pageContext.ProductId, cancellationToken),
            _ => await GetCatalogAsync(cancellationToken)
        };
    }

    private async Task<AiPageData> GetCatalogAsync(CancellationToken cancellationToken)
    {
        var products = await _mediator.Send(new GetProductListQuery(), cancellationToken);
        var catalog = products
            .Where(product => product.Status == ProductStatus.Active)
            .Take(CatalogOverviewLimit)
            .Select(MapSummary)
            .ToArray();

        return new AiPageData(AiPageContextKind.Catalog, catalog, null);
    }

    private async Task<AiPageData> GetProductDetailAsync(int? productId, CancellationToken cancellationToken)
    {
        if (productId is null or <= 0)
        {
            throw new ArgumentException("El contexto de detalle requiere un producto válido.", nameof(productId));
        }

        var product = await _mediator.Send(new GetProductByIdQuery(productId), cancellationToken);
        var similarProducts = await GetSimilarProductsAsync(product, cancellationToken);
        var reviews = product.reviews?
            .Select(review => new AiProductReview(review.Rating, review.Comment))
            .ToArray() ?? [];

        return new AiPageData(
            AiPageContextKind.ProductDetail,
            [],
            new AiProductDetail(MapSummary(product), reviews, similarProducts));
    }

    private async Task<IReadOnlyList<AiProductSummary>> GetSimilarProductsAsync(
        ProductVm product,
        CancellationToken cancellationToken)
    {
        if (product.CategoryId is null)
        {
            return [];
        }

        var minPrice = product.Price is null ? null : product.Price * 0.8m;
        var maxPrice = product.Price is null ? null : product.Price * 1.2m;
        var result = await _mediator.Send(
            new PaginationProductsQuery
            {
                CategoryId = product.CategoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                PageIndex = 1,
                PageSize = SimilarProductsLimit + 1,
                Status = ProductStatus.Active
            },
            cancellationToken);

        return result.Data?
            .Where(candidate => candidate.Id != product.Id)
            .Take(SimilarProductsLimit)
            .Select(MapSummary)
            .ToArray() ?? [];
    }

    private static AiProductSummary MapSummary(Product product)
    {
        return new AiProductSummary(
            product.ProductName,
            product.Description,
            product.Price,
            product.Currency,
            product.Category?.Name,
            product.Rating,
            product.Reviews?.Count ?? 0);
    }

    private static AiProductSummary MapSummary(ProductVm product)
    {
        return new AiProductSummary(
            product.ProductName,
            product.Description,
            product.Price,
            product.Currency,
            product.CategoryNombre ?? product.Category?.Name,
            product.Rating,
            product.NumeroReviews);
    }
}
