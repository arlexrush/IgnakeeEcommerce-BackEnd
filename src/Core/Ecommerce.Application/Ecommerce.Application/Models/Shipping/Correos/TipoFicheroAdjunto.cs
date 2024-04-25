using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoFicheroAdjunto
    {
        // Obligatorio si
        // Nombre del fichero adjunto
        // 100 espacios
        public string? NombreF { get; set; }

        // Obligatorio si
        // Tipo de documento adjunto: 1-.jpg 2-.pdf 3-.zpl
        // 1 espacios
        public string? Tipo_Doc { get; set;}

        // Obligatorio si
        // Fichero propiamente dicho codificado en Base64
        // 100 espacios
        public byte[]? Fichero { get; set;}
    }
}
