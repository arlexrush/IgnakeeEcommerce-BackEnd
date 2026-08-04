using Ecommerce.Application.Features.Addresses.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressCommand : IRequest<ShippingAddressVm>
    {
        public int Id { get; set; }
    }
}
