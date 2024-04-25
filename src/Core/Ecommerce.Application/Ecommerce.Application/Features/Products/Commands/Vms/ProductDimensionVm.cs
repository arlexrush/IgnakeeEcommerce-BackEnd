using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Commands.Vms
{
    public class ProductDimensionVm
    {
        public int Length { get; set; }
        //Ancho en cm
        public int Width { get; set; }
        //Profundidad o Altura en cm
        public int Depth { get; set; }
        //Peso en gramos
        public int Weight { get; set; }
        public int ProductId { get; set; }
    }
}
