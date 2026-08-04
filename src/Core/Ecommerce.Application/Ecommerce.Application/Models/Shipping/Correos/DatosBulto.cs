using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class DatosBulto
    {
        // 1 en envíos monobulto y número de bulto que ocupa en la expedición en envíos multibulto (expedición) / 2 espacios
        public int? Numbulto { get; set; }

        // Código con el que un envío ha quedado preregistrado en Correos. Para envíos nacionales es de 23 posiciones para envíos internacionales de 13. / 23 espacios
        public string? CodEnvio { get; set; }

        // Código de Manifiesto en el que está incluido el envío.

        //Formato.

        //MDCCCCEEAAAAMMDD00000000

        //MD: Manifiesto de deposito

        //CCCC: Código Cliente Etiquetador

        //EE: Canal.En este caso 07.

        //AAAAMMDD: Fecha.

        //Todos los envíos prerregistrados por un cliente etiquetador(CCCC) en un día serán incluidos en un mismo Manifiesto.

        // 24 espacios
        public string? CodManifiesto { get; set; }

        //
        public TipoEtiqueta? Etiqueta { get; set; }

        // Código devuelto por el servicio web de IPC. / 35 espacios
        public string? CodEnvioIpc { get; set; }

    }
}
