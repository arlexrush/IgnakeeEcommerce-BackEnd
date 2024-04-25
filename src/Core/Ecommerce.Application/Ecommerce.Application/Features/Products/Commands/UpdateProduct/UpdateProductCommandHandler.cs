using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public UpdateProductCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductVm> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var findProduct= await _unitOfWork!.Repository<Product>().GetByIdAsync(request.Id);
            if(findProduct is null)
            {
                throw new NotFoundException(nameof(Product));
            }

            _mapper!.Map(request, findProduct, typeof(UpdateProductCommand), typeof(Product));

            var productUpdated=await _unitOfWork.Repository<Product>().UpdateAsync(findProduct);

            if ((request.ImageUrls is not null) || (request.ImageUrls!.Count > 0))
            {
                var imagesToRemove = await _unitOfWork.Repository<Image>().GetAsync(i => i.ProductId == request.Id);
                _unitOfWork.Repository<Image>().DeleteRange(imagesToRemove);

                request.ImageUrls.Select(x => { x.ProductId = request.Id; return x; }).ToList();
                var newImages=_mapper.Map<List<Image>>(request.ImageUrls);
                _unitOfWork.Repository<Image>().AddRange(newImages);

            }
            await _unitOfWork.Complete();

            var response =_mapper.Map<ProductVm>(findProduct);
            return response;
        }
    }
}
