using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Queries.PaginationProducts
{
    public class PaginationProductsQuery:PaginationBaseQuery, IRequest<PaginationVm<ProductVm>>
    {
        public int? CategoryId { get; set; }
        public decimal? MaxPrice { get; set; }  
        public decimal? MinPrice { get; set;}
        public decimal? PrecioPrice { get; set; }
        public int? Rating { get; set; }
        public ProductStatus Status { get; set; }
    }
}
