using Ecommerce.Domain.Commons;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Domain
{
    public class Image : BaseDomainModel
    {
        [MaxLength(4000)]
        public string? Url { get; set; }

        public string? PublicCode { get; set; }

        public int? ProductId { get; set; }

        public virtual Product? Product { get; set; }

    }
}
