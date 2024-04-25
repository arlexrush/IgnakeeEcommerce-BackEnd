using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class DatosBultoError
    {
        // 1 en envíos monobulto y número de bulto que ocupa en la expedición en envíos multibulto (expedición) / 2 espacios
        public int? Numbulto { get; set; }

        // Código de Error encontrado en el preregistro del bulto. Descrito en apartado 3.2.9 / 
        public string? Error { get; set; }

        //Código de Error encontrado en el preregistro del bulto. Descrito en apartado 3.2.9
        public string? DescError { get; set; }
    }
}
