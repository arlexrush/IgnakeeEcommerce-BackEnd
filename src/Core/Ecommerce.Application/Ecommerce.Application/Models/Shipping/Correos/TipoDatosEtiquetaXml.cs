using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoDatosEtiquetaXml
    {
        // Obligatorio si
        //
        public TipoDatosRemitenteEtiqueta? RemitenteEtiqueta;

        // Obligatorio si
        //
        public TipoDatosDestinatarioEtiqueta? DestinatarioEtiqueta;

        // Obligatorio No
        // Referencia propia del cliente.
        // 30 espacios
        public string? Referencia { get; set; }

        // Obligatorio si
        // Peso en gramos a imprimir en la etiqueta.
        // 30 espacios
        public string? PesoReal { get; set; }

        // Obligatorio no
        // Peso Volumetrico  a imprimir en la etiqueta.
        public string? PesoVol { get; set; }

        // Obligatorio no
        // Peso Volumetrico  a imprimir en la etiqueta.
        // 90 espacios
        public string? Observaciones { get; set; }

        // Obligatorio no
        // Fecha en que se han generado los datos de la etiqueta (dd-mm-yyyy)
        // 10 espacios
        public DateTime FechaEtiquetado { get; set; }

        // Obligatorio si.
        public TipoFicheroAdjunto? CodigoBarras { get; set; }

        // Obligatorio no
        // Instrucciones de devolución en caso de no entrega para paquetes internacionales Valores: D: Devolver al remitente A: Tratar como abandonado
        // 1 espacios
        public string? InstruccionesDevolucion { get; set; }

        // Valores añadidos a imprimir en la etiqueta.
        public TipoVAEtiqueta? VA { get; set; }
    }
}
