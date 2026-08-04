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
    public class ShoppingCartItem : BaseDomainModel
    {
        public string? ProductName { get; set; }
        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ProductPicture { get; set; }
        public string? Category { get; set; }
        public Guid? ShoppingCartMasterId { get; set; }
        public int? ShoppingCartId { get; set; }
        public virtual ShoppingCart? ShoppingCart { get; set; }
        public int ProductId { get; set; }
        public int Stock { get; set; }
    }
}
