using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification.Orders
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(OrderSpecificationParams specificationParams)
            : base(x => (string.IsNullOrEmpty(specificationParams.UserName) || x.BuyerName!.Contains(specificationParams.UserName))
              && (!specificationParams.Id.HasValue || x.Id == specificationParams.Id))
        {
            AddInclude(o => o.OrderItems!);
            AddInclude(o => o.ParTaxItems!);
            ApplyPaging(specificationParams.PageSize * (specificationParams.PageIndex - 1), specificationParams.PageSize);
            if (!string.IsNullOrEmpty(specificationParams.Sort))
            {
                switch (specificationParams.Sort)
                {
                    case "createDateAsc":
                        AddOrderBy(x => x.CreatedDate!);
                        break;
                    case "createDateDesc":
                        AddOrderByDescending(x => x.CreatedDate!);
                        break;
                    default:
                        AddOrderBy(x => x.CreatedDate!);
                        break;
                }
            }
            else
            {
                AddOrderByDescending(x => x.CreatedDate!);
            }


        }
    }
}
