using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification.Reviews
{
    public class ReviewSpecification : BaseSpecification<Review>
    {
        public ReviewSpecification(ReviewSpecificationParams reviewParams) : base(r => (!reviewParams.ProductId.HasValue) || (r.ProductId == reviewParams.ProductId))
        {
            ApplyPaging(reviewParams.PageSize * (reviewParams.PageIndex - 1), reviewParams.PageSize);

            if (!string.IsNullOrEmpty(reviewParams.Sort))
            {
                switch (reviewParams.Sort)
                {
                    case "createDateAsc":
                        AddOrderBy(f => f.CreatedDate!);
                        break;

                    case "createDateDesc":
                        AddOrderByDescending(f => f.CreatedDate!);
                        break;

                    default:
                        AddOrderBy(f => f.CreatedDate!);
                        break;

                }
            }
            else
            {
                AddOrderByDescending(f => f.CreatedDate!);
            }



        }
    }
}
