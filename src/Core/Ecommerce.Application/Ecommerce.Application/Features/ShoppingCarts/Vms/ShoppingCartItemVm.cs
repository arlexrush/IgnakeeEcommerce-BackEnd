using Ecommerce.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.ShoppingCarts.Vms
{
    public class ShoppingCartItemVm
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductPicture { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Category { get; set; }
        public int? Stock { get; set; }
        public decimal? TotalLine
        {
            get { return Math.Round(Price * Quantity, 2); }
            set {; }
        }

    }
}
