using AutoMapper;
using Ecommerce.Application.Features.Inventory.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specification.Products;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Inventory.Queries.PaginationInventoryProducts
{
    public class PaginationInventoryProductsQueryHandler : IRequestHandler<PaginationInventoryProductsQuery, PaginationVm<InventoryProductVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaginationInventoryProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<InventoryProductVm>> Handle(PaginationInventoryProductsQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.PageIndex is > 0 ? request.PageIndex.Value : 1;
            var pageSize = request.PageSize > 0 ? request.PageSize : 1;

            var specificationParams = new InventoryProductSpecificationParams
            {
                CategoryId = request.CategoryId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Search = request.Search,
                Sort = request.Sort,
                Status = ProductStatus.Active
            };

            var spec = new InventoryProductSpecification(specificationParams);
            var products = await _unitOfWork.Repository<Product>().GetAllByIdWithSpec(spec);
            var totalProducts = await _unitOfWork.Repository<Product>().CountAsync(new InventoryProductForCountingSpecification(specificationParams));
            var totalPages = totalProducts == 0
                ? 0
                : Convert.ToInt32(Math.Ceiling(totalProducts / (decimal)pageSize));

            return new PaginationVm<InventoryProductVm>
            {
                Count = totalProducts,
                Data = _mapper.Map<IReadOnlyList<InventoryProductVm>>(products),
                PageCount = totalPages,
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultByPage = products.Count
            };
        }
    }
}
