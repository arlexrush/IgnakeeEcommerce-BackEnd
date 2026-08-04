using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Shipping.Correos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.ShippingSeur
{
    public class SeurService : ISeurService
    {
        public async Task<RespuestaPreRegistroEnvio> PreRegistro(PreRegistroEnvio request)
        {
            throw new NotImplementedException();
        }

        public async Task<SolicitudEtiquetaOpResponse> SolicitudEtiquetaOp(SolicitudEtiquetaOpRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<RespuestaCalculaTarifa> CalculaTarifaAsync(CalculaTarifa request)
        {
            throw new NotImplementedException();
        }
    }
}
