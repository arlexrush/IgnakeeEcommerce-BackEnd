using Ecommerce.Application.Features.Inventory.Queries.Vms;
using MediatR;

namespace Ecommerce.Application.Features.Inventory.Queries.GetInventoryProductByCode
{
    public class GetInventoryProductByCodeQuery : IRequest<InventoryProductVm>
    {
        public string ProductCode { get; }

        public GetInventoryProductByCodeQuery(string productCode)
        {
            ProductCode = string.IsNullOrWhiteSpace(productCode)
                ? throw new ArgumentException("Product code is required.", nameof(productCode))
                : productCode.Trim();
        }
    }
}
