using Ecommerce.Application.Features.Countries.Queries.Vm;
using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Taxes.Vms
{
    public class TaxVm
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Percentage { get; set; }
        public int CountryId { get; set; }
        public virtual CountryVm? Country { get; set; }
        public virtual List<TaxByProductVm>? TaxByProducts { get; set; }
        public ApplicationTax ApplicationTax { get; set; }
    }
}
