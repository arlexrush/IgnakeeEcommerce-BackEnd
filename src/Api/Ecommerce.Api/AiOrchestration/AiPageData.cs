namespace Ecommerce.Api.AiOrchestration;

public sealed record AiPageData(
    AiPageContextKind Kind,
    IReadOnlyList<AiProductSummary> Catalog,
    AiProductDetail? Product);

public sealed record AiProductSummary(
    string? Name,
    string? Description,
    decimal? Price,
    string? Currency,
    string? Category,
    int? Rating,
    int ReviewCount);

public sealed record AiProductDetail(
    AiProductSummary Product,
    IReadOnlyList<AiProductReview> Reviews,
    IReadOnlyList<AiProductSummary> SimilarProducts);

public sealed record AiProductReview(int Rating, string? Comment);
