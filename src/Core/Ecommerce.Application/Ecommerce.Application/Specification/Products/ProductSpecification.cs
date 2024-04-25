using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification.Products
{
    public class ProductSpecification:BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecificationParams specificationParams)
            : base(x => (string.IsNullOrEmpty(specificationParams.Search) || (x.ProductName!.Contains(specificationParams.Search)) || (x.Description!.Contains(specificationParams.Search)))
                    && (!specificationParams.CategoryId.HasValue || x.CategoryId == specificationParams.CategoryId)
                    && (!specificationParams.PrecioMin.HasValue || x.Price >= specificationParams.PrecioMin)
                    && (!specificationParams.PrecioMax.HasValue || x.Price <= specificationParams.PrecioMax)
                    && (!specificationParams.PrecioPrice.HasValue || x.Price == specificationParams.PrecioPrice)
                    && (!specificationParams.Status.HasValue || x.Status == specificationParams.Status)
                    )
        {
            AddInclude(p => p.Reviews!);
            AddInclude(p => p.ProductImages!);

            ApplyPaging(specificationParams.PageSize * (specificationParams.PageIndex - 1), specificationParams.PageSize);

            if (!string.IsNullOrEmpty(specificationParams.Sort))
            {
                switch(specificationParams.Sort)
                {
                    case "nombreAsc":
                        AddOrderBy(p => p.ProductName!);
                        break;

                    case "nombreDesc":
                        AddOrderByDescending(p => p.ProductName!);
                        break;

                    case "precioAsc":
                        AddOrderBy(p => p.Price!);
                        break;

                    case "precioDesc":
                        AddOrderByDescending(p => p.Price!);
                        break;

                    case "ratingAsc":
                        AddOrderBy(p => p.Rating!);
                        break;

                    case "ratingDesc":
                        AddOrderByDescending(p => p.Rating!);
                        break;

                    default:
                        AddOrderBy(p => p.CreatedDate!);
                        break;
                }

            }
            else
            {
                AddOrderByDescending(p => p.CreatedDate!);
            }
        }
    }
}
