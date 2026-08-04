using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoIdentificacion
    {
        // Obigatorio Si ,si no informado el campo Empresa / Nombre del Destinatario/Remitente. Prevalece sobre el campo de Empresa./ 300 espacios
        public string? Nombre { get; set; }

        // obligatorio no / Primer apellido / 50 espacios
        public string? Apellido1 { get; set; }

        // // obligatorio no / segundo apellido / 50 espacios
        public string? Apellido2 { get; set; }

        // obligatorio:
        //Remitente: envíos con origen Península/Baleares y destino Canarias, Ceuta o Melilla.También para envíos con origen Canarias, Ceuta y Melilla que vayan fuera de su territorio

        //Destinatario: recomendable para envíos con origen/destino Canarias, Ceuta y Melilla con terceros

        //Número de identificación Fiscal o CIF
        // 15 espacios
        public string? Nif { get; set; }

        // obligatorio SI, si no informado el campo Nombre. Nombre de la empresa. Si este campo está relleno indica que se trata de una empresa. Si viene informado y tambián está informado el campo Nombre prevalece el campo Nombre.  150 espacios.
        public string? Empresa { get; set; }

        // Obligatorio Obligatorio para envíos internacionales,si está relleno el campo empresa. / Persona de Contacto. / 150 espacios
        public string? PersonaContacto { get; set; }
    }
}
