using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.ShoppingCarts.Vms
{
    public class ShoppingCartVm
    {
        public string? ShoppingCartId { get; set; }
        public List<ShoppingCartItemVm>? Items { get; set;}
        public int QuantityItem { get {return Items!.Sum(x=>x.Quantity); } 
                                  set {; } }

        public decimal SubTotal { get { return (Math.Round( (Items!.Sum(x=>x.Price * x.Quantity)),2)); } 
                                  set {; } }

        public decimal Tax { get { return Math.Round(((Items!.Sum(x=>x.Price*x.Quantity))*(Convert.ToDecimal(0.18))),2); } 
                             set {; } }

        public decimal Shipping { get { return Math.Round(((Items!.Sum(x => x.Price * x.Quantity)) < 100 ? 10m : 25m), 2);  } 
                                  set { } }

        public decimal Total { get { return (Math.Round((Items!.Sum(x => x.Price * x.Quantity)), 2)) +
                                            (Math.Round(((Items!.Sum(x => x.Price * x.Quantity)) * (Convert.ToDecimal(0.18))), 2)) +
                                            (Math.Round(((Items!.Sum(x => x.Price * x.Quantity)) < 100 ? 10m : 25m), 2)); } 
                               set { ; }
    }

    }   
}
