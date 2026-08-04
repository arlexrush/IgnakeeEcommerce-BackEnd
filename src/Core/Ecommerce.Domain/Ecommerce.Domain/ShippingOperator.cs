using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Domain
{
    public class ShippingOperator : BaseDomainModel
    {
        public string? NameService { get; set; }
        public string? Type { get; set; }
        public int? OrderId { get; set; }
        [Precision(20, 2)]
        public decimal? TarifaShipping { get; set; }
        public string? NameShippingOperator { get; set; }
        public bool? OperatorStatus { get; set; }
        public string? CountryName { get; set; }
        public virtual Country? Country { get; set; }
    }
}
