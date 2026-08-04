using Ecommerce.Domain.Commons;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Domain
{
    public class Category : BaseDomainModel
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
