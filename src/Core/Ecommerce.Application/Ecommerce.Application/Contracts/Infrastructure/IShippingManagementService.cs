using Ecommerce.Application.Models.Shipping;
using Ecommerce.Application.Models.Shipping.Correos;
using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts.Infrastructure
{
    public interface IShippingManagementService
    {
        public Task<PropertyInformation> SelectShippingTarifa(Domain.Address address, int pesograims, ShoppingCart shoppingCart);
        public Task<SolicitudEtiquetaOpResponse> RequestTagShipping(PropertyInformation service);
        public Task<RespuestaPreRegistroEnvio> DoShipping(PropertyInformation service, User user, OrderAddress address, int? pesograims, Order order);
    }
}
