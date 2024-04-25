using Ecommerce.Application.Features.Shippings.Vms;
using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Order
{
    public class ShippingVm
    {
        public int? OrderId { get; set; }
        public virtual ShippingOperatorVm? Operator { get; set; }
        public decimal? TotalShipping { get; set; }
    }
}
