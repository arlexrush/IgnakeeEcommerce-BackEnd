using AutoMapper;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.CreateShoppingCart
{
    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public CreateShoppingCartCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ShoppingCartVm> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var shoppingCartMasterId = Guid.NewGuid();
            var shoppingcartEntity = await _unitOfWork!.Repository<ShoppingCart>().GetEntityAsync(x => x.ShoppingCartMasterId == shoppingCartMasterId, null, true);

            if (shoppingcartEntity is not null)
            {
                var shoppingCartResponse = _mapper!.Map<ShoppingCartVm>(shoppingcartEntity);
                return shoppingCartResponse;
            }

            var newShoppingCart = new ShoppingCart() { ShoppingCartMasterId = shoppingCartMasterId };
            var shoppingCartEntity = await _unitOfWork.Repository<ShoppingCart>().AddAsync(newShoppingCart);
            var shoppingcartResponse = _mapper!.Map<ShoppingCartVm>(shoppingCartEntity);
            return shoppingcartResponse;
        }
    }
}
