using Ecommerce.Application.Features.Products.Queries.Vms;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand:IRequest<ProductVm>
    {
        public string? ProductName { get; set;}
        public decimal ProductPrice { get; set;}
        public string? ProductDescription { get; set;}
        public string? ProductSeller { get; set;}
        public string? CountrySell { get; set; }
        public int Stock { get; set;}
        public string? CategoryId { get; set;}
        public IReadOnlyList<IFormFile>? ProductRequestImages { get; set;}
        public IReadOnlyList<CreateProductImageCommand>? ImageUrls { get; set;}


    }
}
