using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping
{
    public class PropertyInformation
    {
        public string? NameService { get; set; }
        public string? OperatorName { get; set; }
        public bool? OperatorStatus { get; set; }
        public int? OrderId { get; set; }
        public Type? Type { get; set; }
        public decimal? TarifaShipping { get; set; }
    }
}
