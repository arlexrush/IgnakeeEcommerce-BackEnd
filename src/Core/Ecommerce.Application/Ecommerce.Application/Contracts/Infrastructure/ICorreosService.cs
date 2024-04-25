using Ecommerce.Application.Models.Shipping.Correos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts.Infrastructure
{
    public interface ICorreosService
    {
        public Task<RespuestaPreRegistroEnvio> PreRegistro(PreRegistroEnvio request);

        public Task<SolicitudEtiquetaOpResponse> SolicitudEtiquetaOp(SolicitudEtiquetaOpRequest request);

        public Task<RespuestaCalculaTarifa> CalculaTarifaAsync(CalculaTarifa request);
    }
}
