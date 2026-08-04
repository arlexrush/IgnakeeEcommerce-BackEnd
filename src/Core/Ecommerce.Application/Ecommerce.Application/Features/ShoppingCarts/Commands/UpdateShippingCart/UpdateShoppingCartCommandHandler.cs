using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Org.BouncyCastle.Asn1.Esf;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart
{
    public class UpdateShoppingCartCommandHandler : IRequestHandler<UpdateShoppingCartCommand, ShoppingCartVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public UpdateShoppingCartCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ShoppingCartVm> Handle(UpdateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            //verifing if exist Shopping Cart 
            ShoppingCart shoppingCartUpdate = await _unitOfWork!.Repository<ShoppingCart>().GetEntityAsync(x => x.ShoppingCartMasterId == request.ShoppingCartId);

            if (shoppingCartUpdate is null)
            {
                throw new NoFoundException(nameof(ShoppingCart), request.ShoppingCartId!);
            }

            int result;
            // To get List of items from database where shoppingCartMasterId to match to request
            var shoppingCartItems = await _unitOfWork.Repository<ShoppingCartItem>().GetAsync(x => x.ShoppingCartMasterId == request.ShoppingCartId);

            //to get new items from request, them to map ShoppingCartItem type from ShoppingCartItemVM 
            var shoppingCartItemsToAdd = _mapper!.Map<List<ShoppingCartItem>>(request.ShoppingCartItems);


            if (shoppingCartItems.Count == 0)
            {
                //Setting shopping Cart ids on items of shopping Cart.
                shoppingCartItemsToAdd.ForEach(x =>
                {
                    x.Id = null;
                    x.ShoppingCartId = shoppingCartUpdate.Id;
                    x.ShoppingCartMasterId = request.ShoppingCartId;
                });
                //Adding new items into database
                _unitOfWork.Repository<ShoppingCartItem>().AddRange(shoppingCartItemsToAdd);
                try
                {
                    result = await _unitOfWork!.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }

                if (result <= 0)
                {
                    throw new Exception("Error in Updating ShoppingCart item");
                }
            }
            else
            {
                _unitOfWork.Repository<ShoppingCartItem>().DeleteRange(shoppingCartItems);
                //Setting shopping Cart ids on items of shopping Cart.
                shoppingCartItemsToAdd.ForEach(x =>
                {
                    x.Id = null;
                    x.ShoppingCartId = shoppingCartUpdate.Id;
                    x.ShoppingCartMasterId = request.ShoppingCartId;
                });
                // Update items in database
                await _unitOfWork.Repository<ShoppingCartItem>().UpdateRangeAsync(shoppingCartItemsToAdd);
            }

            // To build response object
            var includes = new List<Expression<Func<ShoppingCart, object>>>();
            includes.Add(x => x.ShoppingCartItems!.OrderBy(x => x.ProductName));
            var shoppingCart = await _unitOfWork!.Repository<ShoppingCart>().GetEntityAsync(x => x.ShoppingCartMasterId == request.ShoppingCartId, includes, true);


            var response = _mapper.Map<ShoppingCartVm>(shoppingCart);
            return response;
        }
    }
}
