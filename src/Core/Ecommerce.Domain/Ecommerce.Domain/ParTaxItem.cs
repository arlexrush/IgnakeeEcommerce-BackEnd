using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Domain
{
    public class ParTaxItem : BaseDomainModel
    {
        public string? TaxName { get; set; }
        [Precision(20, 2)]
        public decimal? TaxPercentage { get; set; }
        [Precision(20, 2)]
        public decimal? MontoItem { get; set; }
        [Precision(20, 2)]
        public decimal? TotalMontoItem { get; set; }
    }
}
