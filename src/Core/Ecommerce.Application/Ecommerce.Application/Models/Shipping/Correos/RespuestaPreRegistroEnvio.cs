using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class RespuestaPreRegistroEnvio
    {
        // Código único que identifica la expedición. / 16 espacios
        public string? CodExpedicion { get; set; }

        // Indica el tipo de entrega de la expedición - S: Tiene entrega parcial - Blanco ó N: No tiene entrega parcial / 1 espacio
        public string? EntregaParcial { get; set; }

        // Fecha en la que se realiza la petición de preregistro de una expedición. Formato: dd-mm-yyyy hh:mm:ss / 20 espacios
        public DateTime? FechaRespuesta { get; set; }

        // Indica si la expedición se ha preregistrado o no. 0 indica que la operación se ha efectuado sin problemas y 1 que ha habido error en alguno de los bultos de la expedición
        public int? Resultado { get; set; }

        // Número de bultos. Valor por defecto 1 / 2 espacios
        public int? TotalBultos { get; set; }

        public DatosBulto? Bulto { get; set; }
        public DatosBultoError? BultoError { get; set; }
        public List<Alerta>? Alertas { get; set; }

        // Idioma de Errores, si tiene el valor (EN) y en el envío tiene errores el tag saldrá en la respuesta y los errores estaran en ingles. De lo contrario saldrá el idioma por defecto (español) y el tag IdiomaErrores no saldrá / 2 espacios
        public string? IdiomaErrores { get; set; }
    }
}
