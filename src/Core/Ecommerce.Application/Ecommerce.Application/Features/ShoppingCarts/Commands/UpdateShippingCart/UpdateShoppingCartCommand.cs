using Ecommerce.Application.Features.ShoppingCarts.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart
{
    public class UpdateShoppingCartCommand : IRequest<ShoppingCartVm>
    {
        public Guid? ShoppingCartId { get; set; }

        public List<ShoppingCartItemVm>? ShoppingCartItems { get; set; }

    }
}
