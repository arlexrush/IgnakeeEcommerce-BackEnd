using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.Inventory.Queries.GetInventoryProductByCode
{
    public class GetInventoryProductByCodeQueryHandler : IRequestHandler<GetInventoryProductByCodeQuery, Features.Inventory.Queries.Vms.InventoryProductVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInventoryProductByCodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Features.Inventory.Queries.Vms.InventoryProductVm> Handle(GetInventoryProductByCodeQuery request, CancellationToken cancellationToken)
        {
            var includes = new List<Expression<Func<Product, object>>>
            {
                product => product.Category!
            };

            var product = await _unitOfWork.Repository<Product>().GetEntityAsync(
                product => product.Status == ProductStatus.Active && product.ProductCode == request.ProductCode,
                includes,
                true);

            if (product is null && InventoryProductCode.TryParseSynthetic(request.ProductCode, out var productId))
            {
                product = await _unitOfWork.Repository<Product>().GetEntityAsync(
                    candidate => candidate.Status == ProductStatus.Active &&
                        candidate.Id == productId &&
                        string.IsNullOrWhiteSpace(candidate.ProductCode),
                    includes,
                    true);
            }

            if (product is null)
            {
                throw new NoFoundException(nameof(Product), request.ProductCode);
            }

            return _mapper.Map<Features.Inventory.Queries.Vms.InventoryProductVm>(product);
        }
    }
}
