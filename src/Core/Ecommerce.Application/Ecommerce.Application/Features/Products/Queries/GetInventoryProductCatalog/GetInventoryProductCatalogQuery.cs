using Ecommerce.Application.Features.Products.Queries.Vms.Inventory;
using Ecommerce.Application.Features.Shared.Queries;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.GetInventoryProductCatalog
{
    /// <summary>
    /// Returns a paginated list of active products for catalog synchronization by the
    /// IgnakeeAI.McpServer.Supplier adapter.
    /// Supports optional text search and category filtering consistent with existing conventions.
    /// </summary>
    public class GetInventoryProductCatalogQuery : PaginationBaseQuery, IRequest<PaginationVm<InventoryProductVm>>
    {
        public int? CategoryId { get; set; }
    }
}
