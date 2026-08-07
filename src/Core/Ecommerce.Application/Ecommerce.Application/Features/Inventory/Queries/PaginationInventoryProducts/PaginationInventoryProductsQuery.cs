using Ecommerce.Application.Features.Inventory.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using MediatR;

namespace Ecommerce.Application.Features.Inventory.Queries.PaginationInventoryProducts
{
    public class PaginationInventoryProductsQuery : PaginationBaseQuery, IRequest<PaginationVm<InventoryProductVm>>
    {
        public int? CategoryId { get; set; }
    }
}
