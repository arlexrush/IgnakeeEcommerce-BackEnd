using Ecommerce.Application.Features.Products.Queries.Vms.Inventory;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.Products.Queries.GetInventoryProductByCode
{
    public class GetInventoryProductByCodeQueryHandler
        : IRequestHandler<GetInventoryProductByCodeQuery, InventoryProductVm?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInventoryProductByCodeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InventoryProductVm?> Handle(
            GetInventoryProductByCodeQuery request,
            CancellationToken cancellationToken)
        {
            var includes = new List<Expression<Func<Product, object>>>
            {
                p => p.Category!,
            };

            var product = await _unitOfWork.Repository<Product>().GetEntityAsync(
                x => x.ProductCode == request.ProductCode && x.Status == ProductStatus.Active,
                includes,
                disableTracking: true);

            if (product is null)
            {
                return null;
            }

            return MapToVm(product);
        }

        public static InventoryProductVm MapToVm(Product product) =>
            new InventoryProductVm
            {
                ProductCode = product.ProductCode,
                ProductId = product.Id,
                ProductName = product.ProductName,
                Description = product.Description,
                Category = product.Category?.Name,
                Price = product.Price,
                Currency = product.Currency,
                Stock = product.Stock,
                UnitToSell = product.UnitToSell,
                PurchaseLeadTime = product.PurchaseLeadTime,
                PurchaseLeadTimeUnit = product.PurchaseLeadTimeUnit,
                Status = product.Status,
            };
    }
}
