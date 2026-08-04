using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification.Orders
{
    public class OrderForCountingSpecification : BaseSpecification<Order>
    {
        public OrderForCountingSpecification(OrderSpecificationParams specificationParams)
            : base(x => (string.IsNullOrEmpty(specificationParams.UserName) || x.BuyerName!.Contains(specificationParams.UserName))
              && (!specificationParams.Id.HasValue || x.Id == specificationParams.Id))
        {

        }
    }
}
