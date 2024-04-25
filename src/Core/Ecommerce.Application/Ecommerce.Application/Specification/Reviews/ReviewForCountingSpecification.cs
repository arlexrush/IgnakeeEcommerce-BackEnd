using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification.Reviews
{
    public class ReviewForCountingSpecification : BaseSpecification<Review>
    {
        public ReviewForCountingSpecification(ReviewSpecificationParams reviewParams) : base(r => (!reviewParams.ProductId.HasValue)||(r.ProductId == reviewParams.ProductId))
        {

        }
    }
}
