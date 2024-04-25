using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class SolicitudEtiquetaOpRequest
    {
        // Obligatorio, SI. En desuso (ver sección Observaciones Generales)
        // Fecha en la que se realiza la petición de preregistro de una expedición. Formato: dd-mm-yyyy hh:mm:ss
        // 20 espacios
        public DateTime? FechaOperacion;

        // Obligatorio, Si.
        // Código que identifica al cliente de Correos. CCCC
        // 4 espacios. 
        public string? CodEtiquetador;

        // Obligatorio, SI (excepto si se ha informado el campo CodEtiquetador)
        // El número de contrato (si corresponde).
        // 8 espacios.
        public string? NumContrato;

        // Obligatorio, SI (excepto si se ha informado el campo CodEtiquetador)
        // El número del cliente  (si corresponde)
        // 8 espacios
        public string? NumCliente;

        // Obligatorio, SI 
        // Código con el que un envío ha quedado preregistrado en Correos
        // 8 espacios
        public string? CodEnvio;

        // Obligatorio, SI (ver sección Observaciones Generales)
        // Código de Agregación Relación de Envíos. Por Defecto 000000
        // 6 espacios
        public string? Care;

        // Obligatorio, SI (ver sección Observaciones Generales)
        // Modo en que solicita la etiqueta en la respuesta de la petición: 1. XML 2. PDF 3. ZPL
        // 1 espacio
        public string? ModDevEtiqueta;

        // Obligatorio, No
        // Idioma de Errores, si tiene el valor (EN) y en el envío tiene errores el tag saldrá en la respuesta y los errores estaran en ingles. De lo contrario saldrá el idioma por defecto (español) y el tag IdiomaErrores no saldrá
        // 2 espacios
        public string? IdiomaErrores;

    }
}
