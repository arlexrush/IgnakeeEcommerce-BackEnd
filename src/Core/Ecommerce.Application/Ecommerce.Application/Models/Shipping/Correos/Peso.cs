using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Correos
{
    public class Peso
    {
        //Obligatorio
        //Real, Volumetrico
        public TipoPeso TipoPeso { get; set; }

        // Obligatorio si
        public int? Valor { get; set; }
    }
}
