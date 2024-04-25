using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoDireccionIpc
    {
        // obligatorio NO /	Nombre identificativo / 40 espacios
        public string? Nombre { get; set; }

        // obligatorio NO / Nombre de la calle. Acepta dos elementos de este tipo consecutivos / 40 espacios
        public string? Calle { get; set; }

        // obligatorio NO / Número de la calle / 8 espacios
        public string? NumeroCalle { get; set; }

        // obligatorio NO / Código postal / 8 espacios
        public string? ApartadoCorreos { get; set; }

        // obligatorio NO / Código ZIP / 9 espacios
        public string? CodigoZip { get; set; }

        // obligatorio NO / Ciudad / 40 espacios
        public string? Ciudad { get; set; }

        // obligatorio NO / Pais / 2 espacios
        public string? Pais { get; set; }

        // obligatorio NO / Nombre de contacto / 40 espacios
        public string? NombreContacto { get; set; }

        // obligatorio NO / Teláfono de contacto / 20 espacios
        public string? TelefonoContacto { get; set; }

        // obligatorio NO / Email de contacto / 30 espacios
        public string? Email { get; set; }
    }
}
