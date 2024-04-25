using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class SolicitudEtiquetaOpResponse
    {
        // Obligado, NO
        // Idioma de Errores, si tiene el valor (EN) y en el envío tiene errores el tag saldrá en la respuesta y los errores estaran en ingles. De lo contrario saldrá el idioma por defecto (español) y el tag IdiomaErrores no saldrá 
        // 2 espacios
        public string? IdiomaErrores;

        // Obligado, NO
        // Código único que identifica la expedición. Únicamente para envíos nacionales.
        // 16 espacios
        public string? CodExpedicion;

        // Obligado, SI
        // Fecha en la que se realiza la petición de preregistro de una expedición. Formato: dd-mm-yyyy hh:mm:ss
        // 20 espacios
        public DateTime FechaRespuesta;

        // Obligado, SI
        // Indica si la expedición se ha preregistrado o no. 0 indica que la operación se ha efectuado sin problemas y 1 que ha habido error en alguno de los bultos de la expedición
        // 1 espacio
        public int? Resultado;

        // Obligado, NO
        // Siempre a 1. A excepción si son bultos de una expedición que llevará el número de bultos de la expedición
        //  2 espacios
        public int? TotalBultos;

        // Obligado si, si resultado = 0
        // 
        public DatosBulto? Bulto;
    }
}
