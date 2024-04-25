using Ecommerce.Application.Features.Taxes.Vms;
using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Countries.Vms
{
    public class CountryVm
    {
        public string? Name { get; set; }

        public string? Iso2 { get; set; }

        public string? Iso3 { get; set; }

        public string? Currency { get; set; }

        public virtual ICollection<TaxVm>? Taxes { get; set; }
    }
}
