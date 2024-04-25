using CloudinaryDotNet.Actions;
using EllipticCurve.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoVAEtiqueta
    {
        // Obligatorio No
        // Importe del Reembolso a imprimir en la etiqueta. Formato ####,##
        // 7 espacios
        public string? ImporteReembolso { get; set; }

        // Obligatorio No
        // Importe del Reembolso a imprimir en la etiqueta. Formato ####,##
        // 7 espacios
        public string? DUA { get; set; }

        // Obligatorio No
        // ‘S’ . En la etiqueta hay que imprimir el literal ‘DUA’ . ‘N’. No se imprime literal ‘DUA’ en etiqueta.
        //  1 espacios
        public string? eAR { get; set; }

        // Obligatorio No
        // ‘S’ . En la etiqueta hay que imprimir el literal ‘Entrega Exclusiva’ ‘N’. No se imprime literal ‘Entrega Exclusiva’ en etiqueta
        // 1 espacios
        public string? EntregaExclusiva { get; set; }

        // Obligatorio No
        // ‘S’ . En la etiqueta hay que imprimir el literal ‘Reparto Sabado’. ‘N’. No se imprime literal ‘Reparto Sabado’ en etiqueta
        // 1 espacios
        public string? RepartoSabado { get; set; }

        // Obligatorio No
        // ‘S’ . En la etiqueta hay que imprimir el literal ‘Entregar’ mas la informacion que va en el campo FechaEntregaConcertada. ‘N’. No se imprime literal ‘Entregar’ en etiqueta
        // 1 espacios
        public string? EntregaConcertada { get; set; }

        // Obligatorio No
        // Fecha de entrerga Formato: dd/mm/aa ‘N’. No se imprime literal ‘Entregar’ en etiqueta
        // 8 espacios
        public string? FechaEntregaConcertada { get; set; }

        // Obligatorio No
        // Si viene valor debe imprimir ‘Entregar (<lo que venga en este tag>)’
        // 20 espacios
        public string? FranjaHorariaConcertada { get; set; }

        // Obligatorio No
        // Solo en envíos Nacionales. - N ó en blanco: Sin entrega con recogida - S: Con entrega con recogida Valor añadido no compatible con ComplejidadGestion.
        // 1 espacios
        public string? EntregaconRecogida { get; set; }

        // Obligatorio No
        // Indica si hay que imprimir la etiqueta LI en la admisión en SGIE. 1: Si hay que imprimir la etiqueta. 0: No hay que imprimir la etiqueta.
        // 1 espacios
        public string? IndImprimirEtiqueta { get; set; }

        // Obligatorio No
        // Texto libre donde el usuario introducirá algún texto de aclaración. El texto será truncado al tamaño máximo.
        // 100 espacios
        public string? TextoAdicional { get; set; }

        // Obligatorio No
        // Tiempo que el envío puede estar en lista. Se admiten valores del 1 al 30. Los valores negativos serán tratados como 0. Se trunca al tamaño máximo. No se admite este elemento vacío.No incluir el elemento si no se especifica un valor. 
        // 2 espacios
        public int TiempoEnLista { get; set; }

        // Obligatorio No
        // Número de intentos de entrega Se admiten valores del 1 al 3. Se trunca al tamaño máximo. No se admite este elemento vacío.No incluir el elemento si no se especifica un valor.
        // 1 espacios
        public int IntentosDeEntrega { get; set; }

        // Obligatorio No
        // Solo en envíos  de producto Paq Standard Internacional. - N: Sin entrega sin firmar - S: Con entrega sin firmar Si se recibe cualquier otro valor se asigna por defecto N.
        // 1 espacios
        public string? EntregaSinFirmar { get; set; }

        // Obligatorio No
        // Solo en envíos Nacionales. Informando este elemento se añade el valor añadido de Gestión necesaria a realizar en Entrega o Admisión dependiendo del producto. Valor añadido no compatible con EntregaconRecogida. Los valores permitidos son: 1:Fácil 2: Medio 3: Complejo 
        // 1 espacios
        public int ComplejidadGestion { get; set; }

        // Obligatorio No
        // Prueba de entrega electrónica. Los valores permitidos son: N: Cuando no hay PEE en los valores añadidos. 3/4/5: Dependerá del dato introducido en la request.
        // 2 espacios
        public string? PEE { get; set; }

        // Obligatorio No
        // S: Se añade el Valor Añadido TarifaPlana N: Sin valor añadido TarifaPlana N
        // 1 espacios
        public string? TarifaPlana { get; set; }
    }
}
