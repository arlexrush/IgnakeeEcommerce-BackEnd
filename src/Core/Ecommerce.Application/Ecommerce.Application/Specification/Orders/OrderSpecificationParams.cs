using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification.Orders
{
    public class OrderSpecificationParams:SpecificationParams
    {
        public string? UserName { get; set; }
        public int? Id { get; set; }
    }
}
