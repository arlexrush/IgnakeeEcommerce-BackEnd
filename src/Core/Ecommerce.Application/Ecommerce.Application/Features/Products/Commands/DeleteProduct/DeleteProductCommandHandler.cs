using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ProductVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public DeleteProductCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductVm> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productToDelete=await _unitOfWork!.Repository<Product>().GetByIdAsync(request.ProductId);
            if (productToDelete is null)
            {
                throw new BadRequestException("This Product doesn´t exist");
            }
            if (productToDelete.Status==ProductStatus.Desactive || productToDelete.Status == ProductStatus.Obsolete)
            {
                throw new Exception("This Product was delected or obsolet");
            }
            productToDelete.Status = ProductStatus.Desactive;
            await _unitOfWork.Repository<Product>().UpdateAsync(productToDelete);
            await _unitOfWork.Complete();
            var response = _mapper!.Map<ProductVm>(productToDelete);
            return response;
        }
    }
}
