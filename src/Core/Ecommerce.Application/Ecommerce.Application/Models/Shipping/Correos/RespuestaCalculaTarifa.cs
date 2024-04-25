using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class RespuestaCalculaTarifa
    {
        public DateTime FechaRespuesta { get; set; }

        //Indica si la expedición se ha preregistrado o no. 0 indica que la operación se ha efectuado sin problemas y 1 que ha habido error en alguno de los bultos de la expedición
        public int Resultado { get; set; }

        // Importe de Tarifa /  10 espacios
        public string? Tarifa { get; set; }


        public string? ErroresValidacion { get; set; }

        // Idioma de Errores, si tiene el valor (EN) y en el envío tiene errores el tag saldrá en la respuesta y los errores estaran en ingles. De lo contrario saldrá el idioma por defecto (español) y el tag IdiomaErrores no saldrá / 2 espacios
        public string? IdiomaErrores { get; set; }
    }
}
