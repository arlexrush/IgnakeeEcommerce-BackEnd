using Ecommerce.Domain.Commons;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Domain
{
    public class OrderAddress : BaseDomainModel
    {
        [MaxLength(4000)]
        public string? UserAddress { get; set; }


        [MaxLength(100)]
        public string? City { get; set; }


        [MaxLength(100)]
        public string? Region { get; set; }


        [MaxLength(100)]
        public string? PostalCode { get; set; }


        [MaxLength(100)]
        public string? UserName { get; set; }


        [MaxLength(100)]
        public string? Country { get; set; }



    }
}
