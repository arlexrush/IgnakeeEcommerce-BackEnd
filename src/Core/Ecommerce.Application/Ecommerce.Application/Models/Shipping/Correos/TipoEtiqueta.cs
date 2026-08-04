using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoEtiqueta
    {
        // Obligatorio Si
        // Modo en el que se devuelve la etiqueta del envío: XML, PDF, ZPL
        // 1 Espacio
        public string? Modo { get; set; }

        // Obligatorio, SI, si Modo=1
        public TipoDatosEtiquetaXml? Etiqueta_xml { get; set; }

    }
}
