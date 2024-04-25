using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoDatosRemitenteEtiqueta
    {
        // Nombre del remitente a imprimir en la etiqueta
        // 50 espacios
        public string? Nombre { get; set; }

        // Dirección del remitente a imprimir en la etiqueta.
        // 72 espacios
        public string? Direccion { get; set; }

        // Localidad del remitente a imprimir en la etiqueta
        // 25 espacios
        public string? Localidad { get; set; }

        // Provincia del remitente a imprimir en la etiqueta
        // 40 espacios
        public string? Provincia { get; set; }

        // Persona de Contacto si remitente es una empresa.
        // 50 espacios
        public string? PersonaContacto { get; set; }

        // Código Postal del remitente a imprimir en la etiqueta.
        // 5 espacios
        public string? CP { get; set; }
    }
}
