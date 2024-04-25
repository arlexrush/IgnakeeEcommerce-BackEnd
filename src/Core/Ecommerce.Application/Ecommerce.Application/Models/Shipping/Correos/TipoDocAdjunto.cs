using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoDocAdjunto
    {
        // obligatorio no / Código de tipo de documento / 3 espacios
        public string? TipoDocumentoAdjunto { get; set; }

        // obligatorio no / Identificador del documento adjunto / 35 espacios
        public string? IdDocumentoAdjunto { get; set; }

        // obligatorio no / Nombre del documento adjunto / 35 espacios
        public string? NombreDocumentoAdjunto { get; set; }
    }
}
