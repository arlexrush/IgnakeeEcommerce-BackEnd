using EllipticCurve.Utils;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class DatosDestinatario
    {
        // Obligatorio si
        public TipoIdentificacion? Identificacion { get; set; }

        // Obligatorio si
        public TipoDDireccion? DatosDireccion { get; set; }

        // Obligatorio no
        public TipoDDireccion? DatosDireccion2 { get; set; }

        //	Código Postal. 5 espacios.
        public string? CP { get; set; }

        // Código postal internacional. Obligatorio para el producto PLINI para Unión Europea y países Kahala (Consultar países Kahala en Anexo III) excluyendo los códigos postales del Anexo X / 10 espacios
        public string? ZIP { get; set; }

        // Código ISO del país de origen (Anexo III) / 2 espacios
        public string? Pais { get; set; }

        // Para envíos internacionales - S: El destino del envío es un apartado postal internacional - Blanco ó N: El destino no es un apartado postal Si el envío es internacional y este campo viene informado a ‘S’, el número de apartado debe indicarse en Destinatario#DatosDireccion#Direccion / 1 espacio
        public string? DestinoApartadoPostalinternacional { get; set;}

        // Si el destino es un apartado postal Nacional. Número de apartado postal del destinatario nacional. /  6 espacios
        public string? ApartadoPostaldestino { get; set; }

        // Cuando sea nacional tiene que comenzar por alguno de estos números (6,7,8,9), tener 9 dígitos en total y solo numéricos / 15 espacios
        public string? Telefonocontacto { get; set; }

        // Dirección de correo electrónico del remitente donde se enviará información del envío si el producto lo tiene contemplado. / 50 espacios
        public string? Email { get; set; }

        // Datos de teláfono móvil de remitente donde se enviarán SMS de información si el producto así lo tiene contemplado. 
        public TipoSMS? DatosSMS { get; set; }


    }
}
