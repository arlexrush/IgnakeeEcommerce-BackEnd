using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Shipping.Mrw
{
    public class MRWSettings
    {
        public string? ApiUrl { get; set; }
        public string? Usuario { get; set; }
        public string? Contraseña { get; set; }
        public string? ApiKey { get; set; }
        public string? NIF { get; set; }
    }
}
