using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Queries.PaginationUsers
{
    public class PaginationUsersQuery : PaginationBaseQuery, IRequest<PaginationVm<User>>
    {
    }
}
