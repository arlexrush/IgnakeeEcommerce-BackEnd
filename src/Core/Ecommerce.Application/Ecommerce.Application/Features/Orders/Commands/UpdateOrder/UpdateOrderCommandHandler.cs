using AutoMapper;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public UpdateOrderCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderVm> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var includes = new List<Expression<Func<Order, object>>>();
            includes.Add(x => x.OrderItems!.OrderBy(x => x.productName));
            var order = await _unitOfWork!.Repository<Order>().GetEntityAsync(x=>x.Id==request.orderId, includes, false);
            order.orderStatus = request.status;

            _unitOfWork.Repository<Order>().UpdateEntity(order);
            var result=await _unitOfWork.Complete();
            if (result<=0)
            {
                throw new Exception("Can´t update order");
            }
            var response = _mapper!.Map<OrderVm>(order);
            return response;
        }
    }
}
