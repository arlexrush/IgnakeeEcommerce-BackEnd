using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoItemOriginal
    {
        // Obligatorio No / Carácter que identifica el envío / 1 espacio
        public string? IdentificadorEnvioOriginal { get; set; }

        // Obligatorio No / Código del envío original / 35 espacio
        public string? IdItemEnvioOriginal { get; set; }

        // Obligatorio No / Código del operador del envío / 40 espacio
        public string? OperadorEnvioOriginal { get; set; }

    }
}
