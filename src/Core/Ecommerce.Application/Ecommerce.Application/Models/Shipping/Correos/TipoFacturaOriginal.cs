using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoFacturaOriginal
    {
        // obligatorio no / Carácter identificador de la factura / 1 espacios
        public string? IdentificadorFacturaOriginal { get; set; }

        // obligatorio no / Numero de la factura / 35 espacios
        public string? NumeroFacturaOriginal { get; set; }

        // obligatorio no / Fecha de la factura AAAAMMDD / 10 espacios
        public DateOnly FechaFacturaOriginal { get; set; }
    }
}
