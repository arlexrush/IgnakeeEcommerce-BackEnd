using AutoMapper;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.ShoppingCarts.Queries
{
    public class GetShoppingCartByIdQueryHandler : IRequestHandler<AddItemShoppingCartCommand, ShoppingCartVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public GetShoppingCartByIdQueryHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ShoppingCartVm> Handle(AddItemShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var includes= new  List<Expression<Func<ShoppingCart, object>>>();
            includes.Add(x => x.ShoppingCartItems!.OrderBy(i=>i.ProductName));

            var shoppingCart=await _unitOfWork!.Repository<ShoppingCart>().GetEntityAsync(x=>x.ShoppingCartMasterId==request.ShoppingCartId, includes, true);
            if (shoppingCart==null)
            {
                shoppingCart = new ShoppingCart
                {
                    ShoppingCartMasterId = request.ShoppingCartId,
                    ShoppingCartItems = new List<ShoppingCartItem>()
                };

                _unitOfWork.Repository<ShoppingCart>().AddEntity(shoppingCart);
                await _unitOfWork.Complete();
            }

            var shoppingCartVm = _mapper!.Map<ShoppingCartVm>(shoppingCart!);

            return shoppingCartVm!;
        }
    }
}
