using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Email
{
    public class EmailSettings
    {
        public string? Email { get; set; }
        public string? AppAPIKey { get; set; }
        public string? BaseUrlClient { get; set; }
    }
}
