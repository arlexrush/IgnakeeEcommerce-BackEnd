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
    public class ShippingOperator : BaseDomainModel
    {
        public string? NameService { get; set; }
        public string? Type { get; set; }
        public int? OrderId { get; set; }
        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal? TarifaShipping { get; set; }
        public string? NameShippingOperator { get; set; }
        public bool? OperatorStatus { get; set; }
        public string? CountryName { get; set; }
        public virtual Country? Country { get; set; }
    }
}
