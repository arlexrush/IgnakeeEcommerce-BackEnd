using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Orders.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<OrderVm>
    {
        public Guid? ShoppingCartId { get; set; }
        public ShippingAddressVm? AddressVm { get; set; }

    }
}
