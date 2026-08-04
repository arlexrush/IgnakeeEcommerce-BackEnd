using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Commands.SendPassword
{
    public class SendPasswordCommand : IRequest<string>
    {
        public string? Email { get; set; }
    }
}
