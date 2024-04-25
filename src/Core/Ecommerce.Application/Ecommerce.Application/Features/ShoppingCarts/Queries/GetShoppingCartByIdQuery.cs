using Ecommerce.Application.Features.ShoppingCarts.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.ShoppingCarts.Queries
{
    public class AddItemShoppingCartCommand:IRequest<ShoppingCartVm>
    {
        public Guid? ShoppingCartId { get; set; }

        public AddItemShoppingCartCommand(Guid? shoppingCartId)
        {
            ShoppingCartId = shoppingCartId?? throw new ArgumentNullException(nameof(shoppingCartId));
        }
    }
}
