using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification.Products
{
    public class ProductSpecificationParams:SpecificationParams
    {
        public int? CategoryId { get; set; }
        public decimal? PrecioMax { get; set; }
        public decimal? PrecioMin { get;set; }
        public decimal? PrecioPrice { get;set; }
        public int? Rating { get; set; }
        public ProductStatus? Status { get; set; }
    }
}
