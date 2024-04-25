using Ecommerce.Application.Features.Auths.Users.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser
{
    public class LoginUserCommand:IRequest<AuthResponse>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
