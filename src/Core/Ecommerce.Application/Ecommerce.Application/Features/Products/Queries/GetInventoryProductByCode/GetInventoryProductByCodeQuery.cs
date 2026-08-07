using Ecommerce.Application.Features.Products.Queries.Vms.Inventory;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.GetInventoryProductByCode
{
    /// <summary>
    /// Returns the inventory view for a single active product identified by its canonical ProductCode.
    /// Returns null when the product does not exist or is not in an externally-available state.
    /// </summary>
    public class GetInventoryProductByCodeQuery : IRequest<InventoryProductVm?>
    {
        public string ProductCode { get; }

        public GetInventoryProductByCodeQuery(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode))
            {
                throw new ArgumentException("ProductCode is required.", nameof(productCode));
            }

            ProductCode = productCode.Trim();
        }
    }
}
