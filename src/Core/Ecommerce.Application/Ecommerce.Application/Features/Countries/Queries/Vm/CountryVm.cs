using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Countries.Queries.Vm
{
    public class CountryVm
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Iso2 { get; set; }
        public string? Iso3 { get; set; }
    }
}
