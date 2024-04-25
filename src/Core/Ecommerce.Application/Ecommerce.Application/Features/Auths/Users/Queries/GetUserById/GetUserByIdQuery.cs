using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserById
{
    public class GetUserByIdQuery:IRequest<AuthResponse>
    {
        public string? UserId { get; set; }

        public GetUserByIdQuery(string? userId)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        }
    }
}
