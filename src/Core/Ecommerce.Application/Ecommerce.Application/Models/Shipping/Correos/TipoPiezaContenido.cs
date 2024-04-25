using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoPiezaContenido
    {
        // Obligatorio No / Número identificativo de la pieza / 2 espacios
        public int? NumeroPieza { get; set; }

        // Obligatorio No / Número de unidades / 6 espacios
        public int? NumeroDeUnidades { get; set; }

        // Obligatorio No / Descripción de la pieza / 256 espacios
        public string? Descripcion { get; set; }

        // Obligatorio No / Valor declarado cuyo patrón es ########.## / 11 espacios
        public decimal? ValorDeclarado { get; set; }

        // Obligatorio No / Peso cuyo patrón es ###.## / 6 espacios
        public decimal? PesoNeto { get; set; }

        // Obligatorio No / Código arancelário / 11 espacios
        public int? PartidaArancelaria { get; set; }

        // Obligatorio No / Código de referencia del artículo / 40 espacios 
        public string? ReferenciaArticulo { get; set; }

        // Obligatorio No / Código que identifica la ubicación original / 2 espacios
        public string? UbicacionOriginal { get; set; }

        // Obligatorio No / Carácter que identifica el envío / 1 espacios
        public string? IdentificadorEnvio { get; set; }

        // Obligatorio No / Carácter que identifica la factura / 1 espacios
        public string? IdentificadorFactura { get; set; }
    }
}
