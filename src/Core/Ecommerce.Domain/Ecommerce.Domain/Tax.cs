using Ecommerce.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public class Tax:BaseDomainModel
    {
        public string? Name { get; set; }
        public decimal? Percentage { get; set; }
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }
        public virtual ICollection<TaxByProduct>? TaxByProducts { get; set; }
        public ApplicationTax ApplicationTax { get; set; }
    }
}
