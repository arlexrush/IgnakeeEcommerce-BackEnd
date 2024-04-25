using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoDatosDestinatarioEtiqueta
    {
        // Nombre del destinatario a imprimir en la etiqueta.
        // 50 espacios
        public string? Nombre { get; set; }

        // Dirección del destinatario a imprimir en la etiqueta.
        // 72 espacios
        public string? Direccion { get; set; }

        // Localidad del destinatario a imprimir en la etiqueta
        // 25 espacios.
        public string? Localidad { get; set; }

        // Provincia del destinatario imprimir en la etiqueta.
        // 40 espacios
        public string? Provincia { get; set; }

        // Obligatorio Si, si el envío es nacional
        // Código Postal del destinatario a imprimir en la etiqueta.
        // 5 espacios
        public string? CP { get; set; }

        // Obligatorio SI, para Unión Europea y destinos KAHALA
        // Código postal internacional. Obligatorio para Unión Europea y países Kahala (Consultar países Kahala en Anexo III)
        // 10 Espacios
        public string? ZIP { get; set; }

        // Obligatorio Si, para envíos internacionales
        // Descripción del País del destinatario a imprimir en la etiqueta.
        // 100 Espacios
        public string? Pais { get; set; }

        // Persona de Contacto si destinatario es una empresa.
        // 50 Espacios
        public string? PersonaContacto { get; set; }

        // Telefono de Contacto del destinatario a imprimir en etiqueta.
        // 12 Espacios
        public string? Telefono { get; set; }
    }
}
