using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain
{
    public class ParTaxItem:BaseDomainModel
    {
        public string? TaxName { get; set; }
        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal? TaxPercentage { get; set; }
        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal? MontoItem { get; set; }
        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal? TotalMontoItem { get; set; }
    }
}
