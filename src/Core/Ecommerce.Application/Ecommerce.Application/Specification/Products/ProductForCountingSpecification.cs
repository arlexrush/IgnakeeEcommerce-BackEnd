using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification.Products
{
    public class ProductForCountingSpecification : BaseSpecification<Product>
    {
        public ProductForCountingSpecification(ProductSpecificationParams specificationParams)
            : base(x => (string.IsNullOrEmpty(specificationParams.Search) || (x.ProductName!.Contains(specificationParams.Search)) || (x.Description!.Contains(specificationParams.Search)))
                    && (!specificationParams.CategoryId.HasValue || x.CategoryId == specificationParams.CategoryId)
                    && (!specificationParams.PrecioMin.HasValue || x.Price >= specificationParams.PrecioMin)
                    && (!specificationParams.PrecioMax.HasValue || x.Price <= specificationParams.PrecioMax)
                    && (!specificationParams.PrecioPrice.HasValue || x.Price == specificationParams.PrecioPrice)
                    && (!specificationParams.Status.HasValue || x.Status == specificationParams.Status)
                    )
        {

        }
    }
}
