using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class Reembolso
    {
        // Obligatorio si / Tipos: - RC: A ingresar en cuenta / 2 espacios
        public string? TipoReembolso { get; set; }

        // Obligatorio si / Importe del reembolso, en cántimos de euro. 900,50 = 090050. El máximo para envíos Nacionales es 1000 €. Para envíos Internacionales si el producto dispone de este valor añadido su valor máximo dependerá del País de destino. / 6 espacios
        public int? Importe { get; set; }

        // Obligatorio SI, si Tipo=”RC”. Se permite la introducción de códigos IBAN. / Número de cuenta para abono del importe del reembolso. Obligatorio si se elige Tipo Reembolso “RC” / 34 espacios
        public string? NumeroCuenta { get; set; }

        // Obligatorio no / Siempre ‘S’ / 1 espacios
        public string? Transferagrupada { get; set; }
    }
}
