using Ecommerce.Application.Features.Countries.Queries.Vm;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Shippings.Vms
{
    public class ShippingOperatorVm
    {
        public string? NameShippingOperator { get; set; }
        public string? NameService { get; set; }
        public string? Type { get; set; }
        public int? OrderId { get; set; }
        public bool? OperatorStatus { get; set; }
        public string? CountryName { get; set; }
        public virtual CountryVm? Country { get; set; }
        public decimal TarifaShipping { get; set; }
    }
}
