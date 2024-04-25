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

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoVA
    {
        // Obligatorio no / Si el envío lleva Seguro. Importe del seguro en cántimos de euro.

            //900,50 = 090050.

            //Envíos Nacionales:

            //Mínimo 0,6 €; Máximo 3000 €

            //Envíos Internacionales:

            //Depende del País de destino.

            // 6 espacios
        public string? ImporteSeguro { get; set; }

        // Obligatorio no /
        public Reembolso? Reembolso { get; set; }

        // Obligatorio no / Solo en envíos Nacionales.
 
            //- N ó en blanco: Sin entrega exclusiva

            //- S: Con entrega exclusiva
            // 1 espacios
        public string? EntregaExclusivaDestinatario { get; set; }

        // Obligatorio no /
        public PruebaEntrega? PruebaEntrega { get; set; }

        // Obligatorio no / Reservado a futuro (N/S) // 1 espacios
        public string? Recogidaadomicilio { get; set; }

        // Obligatorio no / Reservado a futuro (N/S) // 1 espacios
        public string? DevolucionAlbaran { get; set; }

        // Obligatorio no / Reservado a futuro (N/S) // 1 espacios
        public string? RepartoenSabado { get; set; }

        // Obligatorio no / Fecha de la entrega concertada: AAAAMMDD // 8 espacios
        public DateOnly EntregaConcertada { get; set; }

        // Obligatorio no / Solo para paq PREMIUM a domicilio.

        //Valores:

        //    - blanco

        //        - 01 : De 09:00 a 12:00

        //        - 02 : De 12:00 a 15:00

        //        - 03 : De 15:00 a 18:00

        //        - 04 : De 18:00 a 21:00
        //        // 2 espacios
        public string? FranjaHorariaConcertada { get; set; }


        // Obligatorio no / Solo en envíos Nacionales. 

            //- N ó en blanco: Sin entrega con recogida

            //- S: Con entrega con recogida

        //Valor añadido no compatible con ComplejidadGestion.
        // 1 espacios
        public string? EntregaconRecogida { get; set; }


        // Obligatorio no / Indica si hay que imprimir la etiqueta LI en la admisión en SGIE. 

                //1: Si hay que imprimir la etiqueta.

                //0: No hay que imprimir la etiqueta.
                // 1 espacios
        public string? IndImprimirEtiqueta { get; set; }


        // Obligatorio no / Texto libre donde el usuario introducirá algún texto de aclaración. El texto será truncado al tamaño máximo.
        // 100 espacios
        public string? TextoAdicional { get; set; }


        // Obligatorio no / Tiempo que el envío puede estar en lista.

        //Se admiten valores del 0 al 30.

        //Los valores negativos serán tratados como 0.

        //Se trunca al tamaño máximo.

        //No se admite este elemento vacío.No incluir el elemento si no se especifica un valor.
        // 2 espacios
        public int TiempoEnLista { get; set; }


        // Obligatorio no / Número de intentos de entrega Se admiten valores del 1 al 3. Se trunca al tamaño máximo. No se admite este elemento vacío.No incluir el elemento si no se especifica un valor.
        // 1 espacios
        public int IntentosDeEntrega { get; set; }


        // Obligatorio no / Solo en envíos de producto Paq Standard Internacional. 

        //- N: Sin entrega sin firmar

        //- S: Con entrega sin firmar

        //Si se recibe cualquier otro valor se asigna por defecto N.
        // 1 espacios
        public string? EntregaSinFirmar { get; set; }


        // Obligatorio no / Solo en envíos Nacionales. 

        //Informando este elemento se añade el valor añadido de Gestión necesaria a realizar en Entrega o Admisión dependiendo del producto.

        //Valor añadido no compatible con EntregaconRecogida. Los valores permitidos son:

        //    1:Fácil

        //    2: Medio

        //    3: Complejo
        // 1 espacios
        public int ComplejidadGestion { get; set; }


        // Obligatorio no / Informando este elemento se añade el valor añadido de Autorización Previa Entrega, para permitir o cancelar la entrega al destinatario. 

        //Los valores permitidos son:

        //    1: El envío se encuentra pendiente de que el cliente autorice la entrega.

        //    2: El cliente ya ha autorizado la entrega.

        //    3: El cliente ya ha cancelado la entrega.
        // 1 espacios
        public int AutorizacionPreviaEntrega { get; set; }


        // Obligatorio no /S: Se añade el Valor Añadido TarifaPlana N: Sin valor añadido TarifaPlana
        // 1 espacios
        public string? TarifaPlana { get; set; }


        // obligatorio SI, si TarifaPlana=”S” / Identificador del cliente TarifaPlana,  en formato correo electrónico.
        // 1 espacios
        public string? IdClienteTarifaPlana { get; set; }


        // Obligatorio SI, si TarifaPlana=”S” / Código que identifica la operación de la compra mediante Correos TarifaPlana
        // 1 espacios
        public string? IdOperacion { get; set; }
    }
}
