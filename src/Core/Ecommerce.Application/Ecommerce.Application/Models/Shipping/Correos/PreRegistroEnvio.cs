using Stripe;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class PreRegistroEnvio
    {
        // Obligatorio si
        // Fecha en la que se realiza la petición de preregistro de una expedición. Formato: dd-mm-yyyy hh:mm:ss
        public DateTime FechaOperacion { get; set; }

        // Obligatorio si
        // Código que identifica al cliente de Correos. CCCC / 4 espacios
        public string? CodEtiquetador { get; set; }

        // Obligatorio si
        // El número de contrato (si corresponde) / 8 espacios
        public string? NumContrato { get; set; }

        // Obligatorio si
        // El número del cliente  (si corresponde)  / 8 espacios
        public string? NumCliente { get; set; }

        // Obligatorio si
        // Código de Agregación Relación de Envíos. Por Defecto 000000 / 6 espacios
        public string? Care { get; set; }

        // Obligatorio si
        // Siempre a 1. A excepción si son bultos de una expedición que llevará el número de bultos de la expedición. / 2 espacios
        public int? TotalBultos { get; set; }

        // Obligatorio si
        // Modo en que solicita la etiqueta en la respuesta de la petición: 1. XML. 2. PDF 3. ZPL / 3 espacios
        public string? ModDevEtiqueta { get; set; }

        // Obligatorio si
        public DatosRemitente? Remitente { get; set; }

        // Obligatorio si
        public DatosDestinatario? Destinatario { get; set; }

        // Obligatorio si
        public DatosEnvio? Envio { get; set; }

        // Indica el tipo de entrega de la expedición S: Tiene entrega parcial - Blanco ó N: No tiene entrega parcial / 1 espacio
        public string? EntregaParcial { get; set; }

        // Código de la expedición / 16 espacios
        public string? CodExpedicion { get; set; }

        // Código de Manifiesto. Formato: “MD” + CodEtiquetador + “07” + Fecha(YYYYMMDD) + valor numárico 8 posiciones / 24 espacios
        public string? CodManifiesto { get; set; }

        //Idioma de Errores, si tiene el valor(EN) y en el envío tiene errores el tag saldrá en la respuesta y los errores estaran en ingles.De lo contrario saldrá el idioma por defecto (español) y el tag IdiomaErrores no saldrá / 2 espacios
        public string? IdiomaErrores { get; set; }

    }
}
