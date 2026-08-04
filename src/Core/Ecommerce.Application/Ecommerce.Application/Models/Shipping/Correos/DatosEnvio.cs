using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class DatosEnvio
    {
        // // Obligatorio no
        // 1 en envíos monobulto y número de bulto que ocupa en la expedición en envíos multibulto (expedición) / 2 espacios
        public int? NumBulto { get; set; }

        // Obligatorio si
        // Ver productos en Anexo I / 5 espacios
        public string? CodProducto { get; set; }

        // Obligatorio no
        // Referencia propia del cliente para identificar su envío. / 100 espacios
        public string? ReferenciaCliente { get; set; }

        // Obligatorio no
        //Código UIDN / 100 espacios
        public string? ReferenciaCliente2 { get; set; }

        // Obligatorio no
        //Código UIDN / 100 espacios
        public string? ReferenciaCliente3 { get; set; }

        // Obligatorio si
        // - FP: Franqueo pagado - FM: Franqueo maquina - ES: Metálico - ON: Pago online / 2 espacios
        public string? TipoFranqueo { get; set; }

        // Obligatorio NO. Obligatorio si TipoFranqueo = “FM”
        //El numero de máquina si elige como modalidad de pago FM (si corresponde) / 8 espacios
        public string? NumMaquinaFranquear { get; set; }

        // Obligatorio NO. Obligatorio si TipoFranqueo = “FM”
        // Importe franqueado (campo 4 = “FM”) en cántimos de euro. 900,50 = 0000090050 / 10 espacios
        public string? ImporteFranqueado { get; set; }

        // Obligatorio no
        //Código de Promoción para descuento / 10 espacios
        public string? CodPromocion { get; set; }

        // NO. Obligatorio para productos nacionales con modalidad de entrega
        // Obligatorio para Productos con Modalidad de Entrega - ST: estándar para el producto /Domicilio - LS: En Oficina Elegida - OR: En oficina de Referencia del destinatario - CP: CityPaq / 2 espacios
        public string? ModalidadEntrega { get; set; }

        // Obligatorio NO.Obligatorio para ModalidadEntrega Oficina Elegida
        // Codired de la oficina elegida en la modalidad entrega en oficina. En caso de los productos S0236, S0133 y S0293 también se aceptarán códigos de CityPaq de 9 posiciones / 9 espacios
        public string? OficinaElegida { get; set; }

        // Obligatorio Si.
        public List<Peso>? Pesos { get; set; }

        // Obligatorio no
        // Largo del envío en centímetros / 3 espacios
        public int? Largo { get; set; }

        // Obligatorio no
        // alto del envío en centímetros / 3 espacios
        public int? Alto { get; set; }

        // Obligatorio no
        // ancho del envío en centímetros / 3 espacios
        public int? Ancho { get; set; }

        // Obligatorio no
        public TipoVA? ValoresAnadidos { get; set; }

        // Obligatorio no
        // Código embalaje / 23 espacios
        public string? CodigoEmbalajePrepago { get; set; }

        // Obligatorio no
        // Código-red Oficina donde se depositará el envío para ser admitido / 7 espacios
        public string? CodigoPuntoAdmision { get; set; }

        // Obligatorio no
        // Formato AAAAMMDD / 8 espacios
        public DateOnly? FechaDepositoPrevista { get; set; }

        // Obligatorio no
        // Primera línea de observaciones / 45 espacios 
        public string? Observaciones1 { get; set; }

        // Obligatorio no
        // Segunda línea de observaciones / 45 espacios
        public string? Observaciones2 { get; set; }

        // Obligatorio NO. Obligatorio para envíos internacionales.
        // Instrucciones de devolución en caso de no entrega para paquetes internacionales Valores: D: Devolver al remitente A: Tratar como abandonado Por defecto: “Devolver al remitente”. / 1 espacio
        public string? InstruccionesDevolucion { get; set; }

        // Obligatorio no
        // Nombre de documento a escanear: valores en anexo VI. Si el campo AccDocumento1 = 1, los valores posibles son(01,02,03,04,05, 06,07,08,09,10,11,12,13,14,15,16) Si el campo AccDocumento1 = 2, los valores posibles son(01, 02, 03,04,05) / 2 espacios
        public int? Documento1 { get; set; }

        // Obligatorio NO. Obligatorio si Documento1 está relleno.
        // Valores: 1:escanear 2:escanear y validar / 1 espacio
        public int? AccDocumento1 { get; set; }

        // Obligatorio no
        // Observaciones acerca del documento 1 / 100 espacios
        public string? ObsDocumento1 { get; set; }

        // Obligatorio no
        // Nombre de documento a escanear: valores en anexo VI. Solo puede tomar los valores(06,07,08,09,10,11,12,13,14,15,16). /  2 espacios
        public int? Documento2 { get; set; }

        // Obligatorio NO. Obligatorio si Documento2 está relleno.
        // Valores: 1:escanear / 1 espacio
        public int? AccDocumento2 { get; set; }

        // Obligatorio no
        // Observaciones acerca del documento 2 / 100 espacios
        public string? ObsDocumento2 { get; set; }

        // Obligatorio NO, solo puede informarse siempre y cuando está informado Documento2
        // Nombre de documento a escanear: valores en anexo VI. Solo puede tomar los valores(06,07,08,09,10,11,12,13,14,15,16). / 2 espacios
        public int? Documento3 { get; set; }

        // Obligatorio NO. Obligatorio si Documento3 está relleno.
        // Valores: 1:escanear / 1 espacio
        public int? AccDocumento3 { get; set; }

        // Obligatorio no
        // Observaciones acerca del documento 3 / 100 espacios
        public string? ObsDocumento3 { get; set; }

        // Obligatorio no
        // NO, excepto (1) y destino internacional. (1) Datos obligatorios si el envío tiene origen Península/Baleares y destino Canarias, Ceuta o Melilla. Tambián para envíos con origen Canarias, Ceuta y Melilla que vayan fuera de su territorio. Tambián para envíos internacionales.
        public TipoAduana? Aduana { get; set; }

        // Obligatorio no
        // Código del envío de ida asociado al envío de logística inversa / 23 espacios
        public string? CodigoIda { get; set; }

        // Obligatorio no
        // Indica que el envío permite embalaje -S: Permite embalaje - blanco ó N: No permite embalaje / 1 espacio
        public string? PermiteEmbalaje { get; set; }

        // Obligatorio no
        // Fecha tope en la que el envío de logística inversa puede ser admitido. Formato: AAAAMMDD / 20 espacios
        public DateTime? FechaCaducidad { get; set; }

        // Obligatorio no
        // Referencia de la expedición / 30 espacios
        public string? ReferenciaExpedicion { get; set; }

        // Obligatorio no
        // Código de TAP destino / 9 espacios
        public string? CodigoHomepaq { get; set; }

        // Obligatorio no
        // Usuario de acceso a CorreosPaq / 100 espacios
        public string? ToquenIdCorPaq { get; set; }

        // Obligatorio no
        // Indicador de que el envio se admitirá mediante HOMEPAQ o CITYPAQ Valores: S: Admision en HOMEPAQ o CITYPAQ N: Admision por otro medio Por defecto: Admision por otro medio. / 1 espacio
        public string? AdmisionHomepaq { get; set; }

        // Obligatorio no
        // Código del operador postal HomePaq (Anexo V) / 2 espacios
        public string? OperadorPostal { get; set; }

        // Obligatorio no
        // Código de admisión del envío original / 50 espacios
        public string? CodigoEnvioOriginal { get; set; }

        // Obligatorio no
        // Indicador de que es un envío de Ida y Vuelta. Solo debe venir relleno para la operación PreRegistroIdaVuelta. / 1 espacio
        public string? ExisteEnvioVueltaLI { get; set; }

        // Obligatorio no
        // Indicador de si lleva o no seguro el envío de vuelta. -S: Lleva seguro - blanco ó N: No lleva seguro / 1 espacio
        public string? SeguroLI { get; set; }

        // Obligatorio no
        // Importe del seguro del envío de vuelta, en centimos de euro. 900,50 = 090050. / Tendrá contenido en caso de que el indicador anterior valga S. / 10 espacios
        public string? ImporteSeguroLI { get; set; }

        // Obligatorio no
        // Indicador de si lleva o no reembolso el envío de vuelta. -S: Lleva reembolso - blanco ó N: No lleva reembolso / 1 espacio
        public string? ReembolsoLI { get; set; }

        // Obligatorio no
        // Importe del reembolso del envío de vuelta, en centimos de euro. 900,50 = 090050. Tendrá contenido en caso de que el indicador anterior valga S. / 10 espacios
        public string? ImporteReembolsoLI { get; set; }

        // Obligatorio no
        // Tipo de reembolso, en caso de llevar reembolso. Valdrá RC o RT. / 2 espacios
        public string? TipoReembolsoLI { get; set; }

        // Obligatorio no
        // Número de cuenta para abono del importe del reembolso. / 24 espacios
        public string? NumeroCuentaLI { get; set; }

        // Obligatorio no
        // Indica si el destinatario puede modificar los datos de envío. - 0: Modificaciones no permitidas - 1: Modificaciones básicas - 2: Modificaciones completas No se admite definir este elemento vacío. / 1 espacio 
        public string? TipoModificacion { get; set; }

        // Obligatorio no
        public TipoLogisticaInversa? DatosLogisticaInversa { get; set; }

        // Obligatorio NO, excepto (2). (2) Datos obligatorios si el producto es PAQ ESTÁNDAR INTERNACIONAL o PAQ PREMIUM INTERNACIONAL, destino BRASIL y concentrador SINBRA.
        // Indica que la devolución de la etiqueta/CN23 la proporciona el sistema externo SINERLOG. El valor será SINBRA / 4 espacios 
        public string? Concentrador { get; set; }

        // Obligatorio NO, excepto (2). (2) Datos obligatorios si el producto es PAQ ESTÁNDAR INTERNACIONAL o PAQ PREMIUM INTERNACIONAL, destino BRASIL y concentrador SINBRA.
        // 	Texto libre / 50 espacios
        public string? Descripción { get; set; }

        // Obligatorio NO, excepto (2). (2) Datos obligatorios si el producto es PAQ ESTÁNDAR INTERNACIONAL o PAQ PREMIUM INTERNACIONAL, destino BRASIL y concentrador SINBRA.
        // Clasificación para el sistema SINERLOG - regular - special - hazmat / 8 espacios
        public string? Clasificacion { get; set; }

        // Obligatorio NO, excepto (2). (2) Datos obligatorios si el producto es PAQ ESTÁNDAR INTERNACIONAL o PAQ PREMIUM INTERNACIONAL, destino BRASIL y concentrador SINBRA.
        // Importe total de las mercancías de aduana formato de decimales con . (###.##)/ 8 espacios
        public string? ImporteTotal { get; set; }

        // Obligatorio NO, excepto (2). (2) Datos obligatorios si el producto es PAQ ESTÁNDAR INTERNACIONAL o PAQ PREMIUM INTERNACIONAL, destino BRASIL y concentrador SINBRA.
        // Moneda - USD o BRL / 3 espacios
        public string? Moneda { get; set; }

        // Obligatorio NO, excepto (2). (2) Datos obligatorios si el producto es PAQ ESTÁNDAR INTERNACIONAL o PAQ PREMIUM INTERNACIONAL, destino BRASIL y concentrador SINBRA.
        // Modalidad de impuesto para el sistema SINERLOG - 1: DDP - 2: DDU / 1 espacio
        public string? ModalidadImpuestos { get; set; }

    }
}
