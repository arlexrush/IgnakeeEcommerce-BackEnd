using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specification;
using Ecommerce.Application.Specification.Products;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Queries.PaginationProducts
{
    public class PaginationProductsQueryHandler : IRequestHandler<PaginationProductsQuery, PaginationVm<ProductVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaginationProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<ProductVm>> Handle(PaginationProductsQuery request, CancellationToken cancellationToken)
        {
            var productSpecificationParam = new ProductSpecificationParams
            { 
                CategoryId = request.CategoryId,
                PageIndex=request.PageIndex,
                PageSize= request.PageSize,
                PrecioMax= request.MaxPrice,
                PrecioMin= request.MinPrice,
                PrecioPrice= request.PrecioPrice,
                Rating= request.Rating,
                Search= request.Search,
                Sort= request.Sort,
                Status= request.Status,
            };

            var spec = new ProductSpecification(productSpecificationParam);

            var productList=await _unitOfWork.Repository<Product>().GetAllByIdWithSpec(spec);


            var specCount = new ProductForCountingSpecification(productSpecificationParam);

            var totalProducts = await _unitOfWork.Repository<Product>().CountAsync(specCount);


            var rounded = Math.Ceiling((Convert.ToDecimal(totalProducts)) / (Convert.ToDecimal(request.PageSize)));

            var totalPages = Convert.ToInt32(rounded);

            var data=_mapper.Map<IReadOnlyList<ProductVm>>(productList);

            int productsByPage;

            if (totalPages > 0)
            {
                productsByPage = Convert.ToInt32(Math.Floor(Convert.ToDecimal(totalProducts) / Convert.ToDecimal(totalPages)));
            }
            else
            {
                productsByPage = 1;
            }

           

            var pagination = new PaginationVm<ProductVm>
            {
                Count = totalProducts,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                ResultByPage = productsByPage,
            };

            return pagination;

        }
    }
}
