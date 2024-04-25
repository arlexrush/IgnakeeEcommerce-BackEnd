using Ecommerce.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public class TaxByProduct:BaseDomainModel
    {
        public int? ProductId { get; set;}
        public virtual Product? Product { get; set;}
        public int? TaxId { get; set;}  
        public bool IsActivated { get; set; }
        public virtual Tax? Tax { get; set; }
        
    }
}
