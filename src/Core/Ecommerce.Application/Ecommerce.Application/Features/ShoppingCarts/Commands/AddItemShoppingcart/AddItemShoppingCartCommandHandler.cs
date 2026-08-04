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

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.AddItemShoppingcart
{
    public class AddItemShoppingCartCommandHandler : IRequestHandler<AddItemShoppingCartCommand, ShoppingCartVm>
    {
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork;

        public AddItemShoppingCartCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ShoppingCartVm> Handle(AddItemShoppingCartCommand request, CancellationToken cancellationToken)
        {
            //is there ShoppingCart?
            ShoppingCart currentShoppingCart = await _unitOfWork!.Repository<ShoppingCart>().GetEntityAsync(x => x.ShoppingCartMasterId == request.ShoppingCartId);

            if (currentShoppingCart is null)
            {
                throw new ArgumentNullException(nameof(currentShoppingCart));
            }

            var currentItems = currentShoppingCart.ShoppingCartItems!;

            ShoppingCartItem itemToAdd = _mapper.Map<ShoppingCartItem>(request.ShoppingCartItems);
            itemToAdd.Id = null;
            itemToAdd.ShoppingCartMasterId = request.ShoppingCartId;
            itemToAdd.ShoppingCartId = currentShoppingCart.Id;
            ShoppingCartVm response = new ShoppingCartVm();
            try
            {
                var responseItemEntity = await _unitOfWork.Repository<ShoppingCartItem>().AddAsync(itemToAdd);
                var responseItemsEntity = await _unitOfWork.Repository<ShoppingCartItem>().GetAsync(x => x.ShoppingCartMasterId == request.ShoppingCartId);
                var responseItemsEntityVm = _mapper.Map<List<ShoppingCartItemVm>>(responseItemsEntity);


                response.ShoppingCartId = currentShoppingCart.Id.ToString();
                response.Items = responseItemsEntityVm;

            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            return response;

        }
    }
}
