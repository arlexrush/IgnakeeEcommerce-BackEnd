using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class DatosRemitente
    {
        // Obligatorio si
        public TipoIdentificacion? Identificacion { get; set; }

        // Obligatorio si
        public TipoDDireccion? DatosDireccion { get; set; }

        //  Obligatorio si
        //	Código Postal. 5 espacios.
        public string? CP { get; set; }

        // Obligatorio SI, para Unión Europea y orígenes KAHALA excepto códigos postales excluidos
        // Código postal internacional. Obligatorio para el producto PLINI para Unión Europea y países Kahala (Consultar países Kahala en Anexo III) excluyendo los códigos postales del Anexo X / 10 espacios
        public string? ZIP { get; set; }

        // Obligatorio SI para envíos Internacionales
        // Código ISO del país de origen (Anexo III) / 2 espacios
        public string? Pais { get; set; }

        // Obligatorio No
        // Cuando sea nacional tiene que comenzar por alguno de estos números (6,7,8,9), tener 9 dígitos en total y solo numéricos / 15 espacios
        public string? Telefonocontacto { get; set; }

        //Obligatorio Si, para destinatario en envíos origen Península y destino Canarias o Ceuta o Melilla o Andorra. (y viceversa y entre ellos)
        // Dirección de correo electrónico del remitente donde se enviará información del envío si el producto lo tiene contemplado. / 50 espacios
        public string? Email { get; set; }

        // Obligatorio Si , para destinatario en envíos origen Península y destino Canarias o Ceuta o Melilla o Andorra. (y viceversa y entre ellos)
        // Datos de teláfono móvil de remitente donde se enviarán SMS de información si el producto así lo tiene contemplado. 
        public TipoSMS? DatosSMS { get; set; }
    }
}
