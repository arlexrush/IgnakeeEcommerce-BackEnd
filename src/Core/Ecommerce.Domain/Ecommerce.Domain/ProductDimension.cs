using Ecommerce.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public class ProductDimension:BaseDomainModel
    {
        //Largo en cm
        public int Length { get; set; }
        //Ancho en cm
        public int Width { get; set; }
        //Profundidad o Altura en cm
        public int Depth { get; set; }
        //Peso en gramos
        public int Weight { get; set; }
        public int ProductId { get; set; }   
        public virtual Product? product { get; set; }
    }
}
