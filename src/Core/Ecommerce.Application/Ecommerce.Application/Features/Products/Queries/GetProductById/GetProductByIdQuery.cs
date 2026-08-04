using Ecommerce.Application.Features.Products.Queries.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductVm>
    {
        public int? ProductId { get; set; }

        public GetProductByIdQuery(int? productId)
        {

            ProductId = productId == 0 || productId == null ? throw new ArgumentNullException() : productId;
        }
    }
}
