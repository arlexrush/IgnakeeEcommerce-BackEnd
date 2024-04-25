using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Orders.Queries.PaginationOrders
{
    public class PaginationOrdersQuery:PaginationBaseQuery, IRequest<PaginationVm<OrderVm>>
    {
        public string? UserName { get; set; }
        public int? Id { get; set; }
    }
}
