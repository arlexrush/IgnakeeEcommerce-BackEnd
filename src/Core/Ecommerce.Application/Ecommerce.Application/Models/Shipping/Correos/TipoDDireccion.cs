using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoDDireccion
    {
        // obligatorio no. / Indica el tipo de dirección: C, AV, etc. / 3 espacios
        public string? TipoDireccion { get; set; }

        // obligatorio si. / Nombre de la dirección / 100 espacios
        public string? Direccion { get; set; }

        // obligatorio no. / Número de la dirección / 5 espacios
        public string? Numero { get; set; }

        // obligatorio no. / Portal de la dirección/ 5 espacios
        public string? Portal { get; set; }

        // obligatorio no. / Bloque de la dirección / 5 espacios
        public string? Bloque { get; set; }

        // obligatorio no. / Escalera de la dirección / 5 espacios
        public string? Escalera { get; set; }

        // obligatorio no. / Piso de la dirección / 5 espacios
        public string? Piso { get; set; }

        // obligatorio no. / Puerta de la dirección / 5 espacios
        public string? Puerta { get; set; }

        // obligatorio si. Localidad / 100 espacios
        public string? Localidad { get; set; }

        // obligatorio no. / Provincia / 40 espacios
        public string? Provincia { get; set; }

    }
}
