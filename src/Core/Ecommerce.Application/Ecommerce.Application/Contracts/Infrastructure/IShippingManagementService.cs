using Ecommerce.Application.Models.Shipping;
using Ecommerce.Domain;

namespace Ecommerce.Application.Contracts.Infrastructure
{
    public interface IShippingManagementService
    {
        public Task<PropertyInformation> SelectShippingTarifa(Domain.Address address, int pesograims, ShoppingCart shoppingCart);
    }
}
