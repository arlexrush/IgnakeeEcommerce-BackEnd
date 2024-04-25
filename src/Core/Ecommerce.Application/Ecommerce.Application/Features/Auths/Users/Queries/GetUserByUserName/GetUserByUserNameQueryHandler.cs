using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserByUserName
{
    public class GetUserByUserNameQueryHandler : IRequestHandler<GetUserByUserNameQuery, AuthResponse>
    {
        public readonly UserManager<User>? _userManager;

        public GetUserByUserNameQueryHandler(UserManager<User>? userManager)
        {
            _userManager = userManager;
        }

        public async Task<AuthResponse> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var user=await _userManager!.FindByNameAsync(request.UserName!);
            if (user == null)
            {
                throw new Exception("doesn´t finds the User");
            }

            var authResponse = new AuthResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                Avatar = user.AvatarUrl,
                Roles = await _userManager.GetRolesAsync(user),
            };
            return authResponse;

        }
    }
}
