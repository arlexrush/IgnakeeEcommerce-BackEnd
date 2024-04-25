using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Taxes.Vms
{
    public class TaxByProductVm
    {
        public int? ProductId { get; set; }
        public virtual ProductVm? Product { get; set; }
        public int? TaxId { get; set; }
        public virtual TaxVm? Tax { get; set; }
    }
}
