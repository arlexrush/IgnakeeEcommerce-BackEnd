using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoDirecAdicional
    {
        // obligatorio NO / Nombre de la calle. Acepta dos elementos de este tipo consecutivos / 40 espacios
        public string? Calle { get; set; }

        // obligatorio NO / Número de la calle / 8 espacios
        public string? NumeroCalle { get; set; }

        // obligatorio NO / Código postal adicional / 8 espacios
        public string? ApartadoCorreos { get; set; }

    }
}
