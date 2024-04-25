using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification.Reviews
{
    public class ReviewSpecificationParams: SpecificationParams
    {
        public int? ProductId { get; set; }
    }
}
