using AutoMapper;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specification;
using Ecommerce.Application.Specification.Orders;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Orders.Queries.PaginationOrders
{
    public class PaginationOrdersQueryHandler : IRequestHandler<PaginationOrdersQuery, PaginationVm<OrderVm>>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public PaginationOrdersQueryHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OrderVm>> Handle(PaginationOrdersQuery request, CancellationToken cancellationToken)
        {
            var orderSpecificationParams = new OrderSpecificationParams()
            {
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Search = request.Search,
                Sort = request.Sort,
                Id = request.Id,
                UserName = request.UserName
            };

            var spec = new OrderSpecification(orderSpecificationParams);
            var orders = await _unitOfWork!.Repository<Order>().GetAllByIdWithSpec(spec);

            var specCount = new OrderForCountingSpecification(orderSpecificationParams);
            var totalOrders = await _unitOfWork.Repository<Order>().CountAsync(specCount);
            var rounded = Math.Ceiling((Convert.ToDecimal(totalOrders)) / (Convert.ToDecimal(request.PageSize)));
            var totalPage = Convert.ToInt32(rounded);
            var data = _mapper!.Map<IReadOnlyList<Order>, IReadOnlyList<OrderVm>>(orders);
            var ordersByPage = orders.Count();
            var responsePagination = new PaginationVm<OrderVm>()
            {
                Count = totalOrders,
                Data = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                PageCount = totalPage,
                ResultByPage = ordersByPage
            };
            return responsePagination;
        }
    }
}
