using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoLogisticaInversa
    {
        // obligatorio NO /
        public TipoDirecAdicional? DatosAdicRemitente { get; set; }

        // obligatorio NO /
        public TipoDirecAdicional? DatosAdicDestinatario { get; set; }

        // obligatorio NO /
        public TipoDireccionIpc? DireccionPostal { get; set; }

        // obligatorio NO / Código de la oficina de intercambio destino / 6 espacios
        public string? OficinaIntercambioDestino { get; set; }

        // obligatorio NO / Referencia cliente / 30 espacios
        public string? ReferenciaClienteIPC { get; set; }

        // obligatorio NO /
        public TipoDireccionIpc? DireccionImportador { get; set; }

        // obligatorio NO /Referencia del importador / 35 espacios
        public string? ImportadorReferencia { get; set; }

        // obligatorio NO / Fax del importadorv / 20 espacios
        public string? ImportadorNumeroFax { get; set; }

        //
        public TipoDireccionIpc? DireccionRepresentanteAceptante { get; set; }

        // obligatorio NO / obligatorio NO / Código del representante VAT / 35 espacios
        public string? RepresentanteAceptanteNumeroVAT { get; set; }

        // obligatorio NO /
        public TipoAduanaIpc? Aduana { get; set; }

        // obligatorio NO / Texto representativo de la razón de devolución / 30 espacios
        public string? RazonDevolucion { get; set; }

        //  obligatorio NO /
        public List<Personalizado>? Personalizados { get; set; }

        // obligatorio NO / Indica si ha sido recogido o no. Valores’S’ o ‘N’ / 1 espacio
        public string? Recogido { get; set; }
    }
}
