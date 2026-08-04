using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Domain
{
    public class Shipping : BaseDomainModel
    {
        public int? OrderId { get; set; }
        public virtual ShippingOperator? Operator { get; set; }

        [Precision(20, 2)]
        public decimal? TotalShipping { get; set; }

        public void SetShippingCost(decimal? totalShipping)
        {
            if (totalShipping is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(totalShipping), "Shipping cost cannot be negative.");
            }

            TotalShipping = totalShipping;
        }

        public void AssignOperator(ShippingOperator? shippingOperator)
        {
            Operator = shippingOperator;
        }

        public bool IsReadyForFulfillment()
        {
            return Operator is not null && TotalShipping is not null;
        }
    }
}
