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

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.DeleteItemShoppingCart
{
    public class DeleteItemShoppingCartCommandHandler : IRequestHandler<DeleteItemShoppingCartCommand, ShoppingCartVm>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteItemShoppingCartCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ShoppingCartVm> Handle(DeleteItemShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var itemToDelete = await _unitOfWork.Repository<ShoppingCartItem>().GetEntityAsync(x => x.Id == request.Id);
            var shoppingCartId = itemToDelete.ShoppingCartMasterId;
            if (itemToDelete == null)
            {
                throw new Exception("Item don´t found");
            }
            await _unitOfWork.Repository<ShoppingCartItem>().DeleteAsync(itemToDelete!);

            var itemsAfterDelete = await _unitOfWork.Repository<ShoppingCartItem>().GetAsync(x => x.ShoppingCartMasterId == shoppingCartId);
            var itemsAfterDeleteVm = _mapper.Map<List<ShoppingCartItemVm>>(itemsAfterDelete);


            ShoppingCartVm response = new ShoppingCartVm()
            {
                ShoppingCartId = shoppingCartId.ToString(),
                Items = itemsAfterDeleteVm
            };

            return response;
        }


    }
}
