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
    public class Country:BaseDomainModel
    {
        
        public string? Name { get; set; }
        
        public string? Iso2 { get; set; }
        
        public string? Iso3 { get; set; }

        public string? Currency { get; set; }

        public virtual ICollection<Tax>? Taxes { get; set; }
    }
}
