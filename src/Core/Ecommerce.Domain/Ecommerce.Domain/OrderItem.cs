using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public class OrderItem:BaseDomainModel
    {
        public Product? Product { get; set; }
        public int ProductId { get; set; }
        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public virtual Order? Order { get; set; }   
        public int OrderId { get; set; }
        public int ProductItemId { get; set; }
        public string? productName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
