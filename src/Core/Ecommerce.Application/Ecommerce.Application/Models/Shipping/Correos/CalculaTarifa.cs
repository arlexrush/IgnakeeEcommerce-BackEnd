

using System.Collections.Generic;
using System.Web;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class CalculaTarifa
    {
        // Fecha en la que se realiza la petición de preregistro de una expedición. Formato: dd-mm-yyyy hh:mm:ss  / 20 espacios
        public DateTime FechaOperacion { get; set; }

        //Código que identifica al cliente de Correos.CCCC  /  4 espacios
        public string? CodEtiquetador { get; set; }

        // Codigo Postal Remitente / 5 espacios
        public string? CPRemitente { get; set; }

        //Codigo Postal Destinatario / 5 espacios
        public string? CPDestinatario { get; set; }

        //Ver productos en Anexo I en documentacion  5 espacios
        public string? CodProducto { get; set; }

        // Tipo de peso: R.Real / V.Volumátrico / 1 espacio
        public string? TipoPeso { get; set; }

        // Peso total en gramos del envío  /  5 espacios
        public int? Valor { get; set; }

        // Idioma de Errores, si tiene el valor (EN) y en el envío tiene errores el tag saldrá en la respuesta y los errores estaran en ingles. De lo contrario saldrá el idioma por defecto (español) y el tag IdiomaErrores no saldrá / 2 espacios
        public string? IdiomaErrores { get; set; }


        public IEnumerable<KeyValuePair<string, string>> ToFormUrlEncoded()
        {
            var formData = new Dictionary<string, string>();

            formData["FechaOperacion"] = FechaOperacion.ToString("yyyy-MM-ddTHH:mm:ss");
            formData["CodEtiquetador"] = CodEtiquetador!;
            formData["CPRemitente"] = CPRemitente!;
            formData["CPDestinatario"] = CPDestinatario!;
            formData["CodProducto"] = CodProducto!;
            formData["Peso"] = TipoPeso!;
            formData["Valor"] = Valor!.ToString()!;
            formData["IdiomaErrores"] = IdiomaErrores ?? "es";


            return formData;
        }
    }
}
