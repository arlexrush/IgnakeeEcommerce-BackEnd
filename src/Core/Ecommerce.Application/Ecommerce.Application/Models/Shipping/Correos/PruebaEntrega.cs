using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class PruebaEntrega
    {
        // Obligatorio no / - 0: Sin prueba de entrega - 2: AR Papel.En desuso.Se utiliza prueba de entrega (3) - 3: Prueba de entrega electrónica - 4: Prueba electrónica(Custodia 10 años) - 5: Prueba electrónica(Custodia 15 años) / 2 espacios
        public int? Formato { get; set; }

        // Obligatorio no / Texto para imprimir en el encabezado del aviso de recibo. En desuso. / 55 espacios
        public string? ReferenciaeAR { get; set; }

        // Obligatorio no / Texto para imprimir, en 5 líneas de 70 caracteres cada una, en el pie del aviso de recibo. En desuso. / 350 espacios
        public string? InfRemitenteEAr { get; set; }
    }
}
