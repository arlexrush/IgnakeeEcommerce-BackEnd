using Ecommerce.Domain;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts.Identity
{
    public interface IAuthService
    {

        public string GetSessionUser();
        public string CreateToken(User user, IList<string>? roles);

    }
}
