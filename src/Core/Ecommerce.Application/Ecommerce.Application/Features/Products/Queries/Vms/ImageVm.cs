using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Queries.Vms
{
    public class ImageVm
    {
        public int? Id { get; set; }
        public string? Url { get; set; }

        public string? PublicCode { get; set; }

        public int? ProductId { get; set; }        
    }
}
