using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoPersonalizado
    {
        // Obligatorio no / Clave del tipo / 40 espacios
        public string? Clave { get; set; }

        // Obligatorio no / Valor asignado / 40 espacios
        public string? Valor { get; set; }
    }
}
