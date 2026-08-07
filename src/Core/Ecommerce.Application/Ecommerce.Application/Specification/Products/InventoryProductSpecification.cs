using Ecommerce.Domain;

namespace Ecommerce.Application.Specification.Products
{
    public class InventoryProductSpecification : BaseSpecification<Product>
    {
        private const string NameAscending = "nombreAsc";
        private const string NameDescending = "nombreDesc";
        private const string PriceAscending = "precioAsc";
        private const string PriceDescending = "precioDesc";

        public InventoryProductSpecification(InventoryProductSpecificationParams specificationParams)
            : base(product =>
                (string.IsNullOrEmpty(specificationParams.Search) ||
                    (product.ProductName != null && product.ProductName.Contains(specificationParams.Search)) ||
                    (product.Description != null && product.Description.Contains(specificationParams.Search)) ||
                    (product.ProductCode != null && product.ProductCode.Contains(specificationParams.Search))) &&
                (!specificationParams.CategoryId.HasValue || product.CategoryId == specificationParams.CategoryId) &&
                (!specificationParams.Status.HasValue || product.Status == specificationParams.Status))
        {
            AddInclude(product => product.Category!);
            ApplyPaging(specificationParams.PageSize * (specificationParams.PageIndex - 1), specificationParams.PageSize);

            if (!string.IsNullOrEmpty(specificationParams.Sort))
            {
                switch (specificationParams.Sort)
                {
                    case NameAscending:
                        AddOrderBy(product => product.ProductName!);
                        break;
                    case NameDescending:
                        AddOrderByDescending(product => product.ProductName!);
                        break;
                    case PriceAscending:
                        AddOrderBy(product => product.Price!);
                        break;
                    case PriceDescending:
                        AddOrderByDescending(product => product.Price!);
                        break;
                    default:
                        AddOrderBy(product => product.ProductName!);
                        break;
                }
            }
            else
            {
                AddOrderBy(product => product.ProductName!);
            }
        }
    }
}
