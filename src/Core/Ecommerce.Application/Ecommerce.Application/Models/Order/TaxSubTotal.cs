using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Order
{
    public class TaxSubTotal
    {
        public string? TaxName { get; set; }
        public decimal? SubTotal { get; set; }
    }
}
