using Ecommerce.Domain;

namespace Ecommerce.Application.Specification.Products
{
    public class InventoryProductForCountingSpecification : BaseSpecification<Product>
    {
        public InventoryProductForCountingSpecification(InventoryProductSpecificationParams specificationParams)
            : base(product =>
                (string.IsNullOrEmpty(specificationParams.Search) ||
                    (product.ProductName != null && product.ProductName.Contains(specificationParams.Search)) ||
                    (product.Description != null && product.Description.Contains(specificationParams.Search)) ||
                    (product.ProductCode != null && product.ProductCode.Contains(specificationParams.Search))) &&
                (!specificationParams.CategoryId.HasValue || product.CategoryId == specificationParams.CategoryId) &&
                (!specificationParams.Status.HasValue || product.Status == specificationParams.Status))
        {
        }
    }
}
