using Ecommerce.Application.Features.Products.Queries.GetInventoryProductByCode;
using Ecommerce.Application.Features.Products.Queries.Vms.Inventory;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specification.Products;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.GetInventoryProductCatalog
{
    internal class GetInventoryProductCatalogQueryHandler
        : IRequestHandler<GetInventoryProductCatalogQuery, PaginationVm<InventoryProductVm>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInventoryProductCatalogQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationVm<InventoryProductVm>> Handle(
            GetInventoryProductCatalogQuery request,
            CancellationToken cancellationToken)
        {
            var specParams = new ProductSpecificationParams
            {
                CategoryId = request.CategoryId,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort,
                Status = ProductStatus.Active,
            };

            var spec = new ProductSpecification(specParams);
            var products = await _unitOfWork.Repository<Product>().GetAllByIdWithSpec(spec);

            var countSpec = new ProductForCountingSpecification(specParams);
            var total = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

            var rounded = Math.Ceiling((decimal)total / (decimal)request.PageSize);
            var totalPages = (int)rounded;

            int resultByPage = products.Count;

            var data = products
                .Select(GetInventoryProductByCodeQueryHandler.MapToVm)
                .ToList()
                .AsReadOnly();

            return new PaginationVm<InventoryProductVm>
            {
                Count = total,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                ResultByPage = resultByPage,
            };
        }
    }
}
