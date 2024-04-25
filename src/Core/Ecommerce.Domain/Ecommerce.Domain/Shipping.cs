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
    public class Shipping:BaseDomainModel
    {
        public int? OrderId { get; set; }
        public virtual ShippingOperator? Operator { get; set; }
        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal? TotalShipping { get; set; }

    }
}
