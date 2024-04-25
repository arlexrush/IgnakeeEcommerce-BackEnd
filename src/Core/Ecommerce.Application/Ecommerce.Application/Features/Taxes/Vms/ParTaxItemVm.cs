using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Taxes.Vms
{
    public class ParTaxItemVm
    {
        public string? TaxName { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? MontoItem { get; set; }
        public decimal? TotalMontoItem { get { return Math.Round((decimal)((TaxPercentage! / 100) * MontoItem!), 2); } set {; } }
    }
}
