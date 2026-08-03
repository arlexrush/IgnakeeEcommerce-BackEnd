using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Products.Commands.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Commands.AddDimensionsToProduct
{
    public class AddDimensionsToProductCommandHandler : IRequestHandler<AddDimensionsToProductCommand, ProductDimensionVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;
        private readonly IAuthService? _authService;

        public AddDimensionsToProductCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper, IAuthService? authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<ProductDimensionVm> Handle(AddDimensionsToProductCommand request, CancellationToken cancellationToken)
        {
            var newProductDimension = new ProductDimension()
            {
                ProductId = request.ProductId,
                Length = request.Length,
                Width = request.Width,
                Depth = request.Depth,
                Weight = request.Weight,
                CreatedBy = _authService!.GetSessionUser(),
                CreatedDate = DateTime.UtcNow
            };
            
            try
            {
                var tesProductDimension = await _unitOfWork!.Repository<ProductDimension>().GetEntityAsync(x=>x.ProductId==newProductDimension.ProductId,null, true);
                if(tesProductDimension is null)
                {
                    var productDimensionEntity = await _unitOfWork!.Repository<ProductDimension>().AddAsync(newProductDimension);
                    var productDimensionResponse = _mapper!.Map<ProductDimensionVm>(productDimensionEntity);
                    return productDimensionResponse;
                }
                else
                {
                    newProductDimension.Id = tesProductDimension.Id;
                    var productDimensionEntity = await _unitOfWork!.Repository<ProductDimension>().UpdateAsync(newProductDimension);
                    var productDimensionResponse = _mapper!.Map<ProductDimensionVm>(productDimensionEntity);
                    return productDimensionResponse;
                }
            }
            catch (Exception)
            {
                throw;
            }

            
        }
    }
}
