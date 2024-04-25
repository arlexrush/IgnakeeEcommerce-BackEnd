using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoAduanaIpc
    {
        // obligatorio NO / Modena utilizada / 3 espacios
        public string? Moneda { get; set; }

        // obligatorio NO /
        public List<PiezasContenido>? PiezasDelContenido { get; set; }

        // obligatorio NO /
        public List<ItemsOriginales>? ItemsOrginales { get; set; }

        // obligatorio NO /
        public List<DocAdjuntos>? DocumentosAdjuntos { get; set; }

        // obligatorio NO /
        public List<FactOriginal>? FacturasOriginales { get; set; }

        // obligatorio NO / Código de la transacción / 3 espacios
        public string? NaturalezaCodigoTransaccion { get; set; }

        // obligatorio NO / Comentarios sobre los elementos declarados / 150 espacios
        public string? Comentarios { get; set; }

        // obligatorio NO / Código Incoterm / 3 espacios
        public string? Incoterm { get; set; }

    }
}
