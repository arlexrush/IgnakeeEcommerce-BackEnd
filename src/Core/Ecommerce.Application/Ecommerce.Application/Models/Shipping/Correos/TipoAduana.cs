using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoAduana
    {
        // Obligatorio si / Tipo de envío: - 1: Documentos - 2: Mercancías - 3: Regalo - 4: Muestras Comerciales - 5: Mercancía Devuelta - 6: Otro - 7: Mercancías Peligrosas / 1 espacios
        public string? TipoEnvio { get; set; }

        // Obligatorio no / - N o blanco: El envío no es un envío comercial - S: El envío es un envío comercial / 1 espacios
        public string? EnvioComercial { get; set; }

        // Obligatorio NO, excepto si EnvioComercial = “S” / - N o blanco: El envío lleva asociada una factura igual o inferior a 500 euros - S: El envío lleva asociada una factura superior a 500 euros / 1 espacios
        public string? FacturaSuperiora500 { get; set; }

        // Obligatorio no, excepto si EnvioComercial y FacturaSuperiora500 = “S” / - N o blanco: En caso de requerir DUA de exportación, NO desea que se lo tramite Correos - S: Desea que el DUA de exportación sea tramitado por Correos / 1 espacios
        public string? DUAConCorreos { get; set; }

        // Obligatorio no, Para envíos internacionales, también cuando aparezca como destino/origen Canarias, Ceuta y Melilla, excepto en envíos entre islas y de Ceuta, Melilla a ellas mismas. / Descripción de los objetos contenidos en el paquete / 
        public List<DescAduanera>? DescAduanera { get; set; }

        // Obligatorio no / Ambos. - N o blanco: No se adjunta factura al envío - S: Se adjunta factura al envío Si se recibe cualquier otro valor se asigna N por defecto. / 1 espacios
        public string? Factura { get; set; }

        // Obligatorio no / Descripción para campo Factura. Se trunca al tamaño máximo. / 15 espacios
        public string? TxtFactura { get; set; }

        // Obligatorio no / Internacional. - N o blanco: No se adjunta licencia al envío - S: Se adjunta licencia al envío Si se recibe cualquier otro valor se asigna N por defecto. / 1 espacios
        public string? Licencia { get; set; }

        // Obligatorio no / Descripción para campo Licencia. Se trunca al tamaño máximo. / 15 espacios
        public string? TxtLicencia { get; set; }

        // Obligatorio no / Internacional. - N o blanco: No se adjunta certificado al envío - S: Se adjunta certificado al envío Si se recibe cualquier otro valor se asigna N por defecto. / 1 espacios
        public string? Certificado { get; set; }

        // Obligatorio no / Descripción para campo Certificado. Se trunca al tamaño máximo. /   15  espacios
        public string? TxtCertificado { get; set; }

        // Obligatorio no / Referencia Aduanera Expedidor. Se trunca al tamaño máximo. / 50 espacios
        public string? RefAduaneraExpedidor { get; set; }

        // Obligatorio no / Referencia Fiscal Importador. Se trunca al tamaño máximo. / 50 espacios
        public string? RefFiscalImportador { get; set; }

        // Obligatorio no / Número IVA Importador. Se trunca al tamaño máximo. / 50 espacios
        public string? NumIvaImportador { get; set; }

        // Obligatorio no / Código Importador. Se trunca al tamaño máximo. / 50 espacios
        public string? CodImportador { get; set; }

        // Obligatorio no / Número de teláfono de importador. Se trunca al tamaño máximo. / 26 espacios
        public string? NumTelefonoImportador { get; set; }

        // Obligatorio no / Email del importador. / 10 espacios
        public string? DesEmailImportador { get; set; }
    }
}
