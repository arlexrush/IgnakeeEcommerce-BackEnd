using Ecommerce.Domain;

namespace Ecommerce.Application.Specification.Products
{
    public class InventoryProductSpecificationParams : SpecificationParams
    {
        public int? CategoryId { get; set; }
        public ProductStatus? Status { get; set; }
    }
}
