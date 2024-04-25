using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts.Infrastructure
{
    public interface IGlovoService
    {
        public Task<string> UploadMenu(Order order);
    }
}
