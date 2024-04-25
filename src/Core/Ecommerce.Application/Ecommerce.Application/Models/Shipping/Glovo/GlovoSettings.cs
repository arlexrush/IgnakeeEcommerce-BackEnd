using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Glovo
{
    public class GlovoSettings
    {
        public string? GlovoApiKey { get; set; }
        public string? UrlPath { get; set; }
        public string? StoreId { get; set; }
    }
}
