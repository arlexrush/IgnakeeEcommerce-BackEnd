using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public CreateProductCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductVm> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product=_mapper!.Map<Product>(request);
            var result= await _unitOfWork!.Repository<Product>().AddAsync(product);
            if ((request.ImageUrls is not null) && (request.ImageUrls.Count > 0))
            {
                var imagesToSave=request.ImageUrls.Select(c => { c.ProductId = product.Id; return c; }).ToList();
                var images= _mapper!.Map<List<Image>>(imagesToSave);
                _unitOfWork.Repository<Image>().AddRange(images);
                await _unitOfWork.Complete();
            } 
            var newProduct=_mapper.Map<ProductVm>(product);
            return newProduct;
        }
    }
}
