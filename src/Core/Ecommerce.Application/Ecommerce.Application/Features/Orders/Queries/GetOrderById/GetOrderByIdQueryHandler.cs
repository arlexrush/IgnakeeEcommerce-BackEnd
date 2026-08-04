using AutoMapper;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public GetOrderByIdQueryHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderVm> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var includes = new List<Expression<Func<Order, object>>>();
            includes.Add(x => x.OrderAddress!);
            includes.Add(x => x.OrderItems!);
            includes.Add(x => x.ParTaxItems!);

            Order order = null;

            try
            {
                order = await _unitOfWork!.Repository<Order>().GetEntityAsync(o => o.Id == request.OrderId, includes, false);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
            }

            var response = _mapper!.Map<OrderVm>(order);

            return response;
        }
    }
}
