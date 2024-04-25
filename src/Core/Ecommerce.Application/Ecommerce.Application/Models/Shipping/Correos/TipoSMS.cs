using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class TipoSMS
    {
        // Obligatorio No
        //Numero de móvil al que enviar información del envío vía SMS
        // 9 Espacios
        public string? NumeroSMS { get; set; }

        // Obligatorio No
        // Idioma en el que se enviarán los SMS. ‘1’ – Castellano. ‘2’ – Catalán. ‘3’ – Euskera. ‘4’ – Gallego ‘6’ – Portuguás(sólo destinatario) 
        // 1 Espacios
        public string? Idioma { get; set; }
    }
}
