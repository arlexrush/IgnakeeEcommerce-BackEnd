using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public class Shipping : BaseDomainModel
    {
        public int? OrderId { get; set; }
        public virtual ShippingOperator? Operator { get; set; }

        [Column(TypeName = "DECIMAL(20,2)")]
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
