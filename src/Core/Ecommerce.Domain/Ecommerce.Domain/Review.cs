using Ecommerce.Domain.Commons;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Domain
{
    public class Review : BaseDomainModel
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        public int Rating { get; set; }
        [MaxLength(4000)]
        public string? Comment { get; set; }
        public int ProductId { get; set; }
        public Product? product { get; set; }

    }
}
