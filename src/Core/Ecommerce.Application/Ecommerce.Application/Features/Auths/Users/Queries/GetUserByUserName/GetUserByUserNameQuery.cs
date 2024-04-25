using Ecommerce.Application.Features.Auths.Users.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserByUserName
{
    public class GetUserByUserNameQuery:IRequest<AuthResponse>
    {
        public string? UserName { get; set;}

        public GetUserByUserNameQuery(string? userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
