using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthService _authService;

        public UpdateUserCommandHandler(UserManager<User> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IAuthService authService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var updateUser = await _userManager.FindByNameAsync(_authService.GetSessionUser());
            if (updateUser is null)
            {
                throw new BadRequestException("The User don´t Exist");
            }

            updateUser.Name = request.Name;
            updateUser.LastName = request.LastName;
            updateUser.Email = request.Email;
            updateUser.PhoneNumber = request.Phone;
            updateUser.AvatarUrl = request.PhotoUrl ?? updateUser.AvatarUrl;

            var result = await _userManager.UpdateAsync(updateUser);

            if (!result.Succeeded)
            {
                throw new Exception("It can´t update your User");
            }

            var userByEmail = await _userManager.FindByEmailAsync(request.Email!);
            var roles = await _userManager.GetRolesAsync(userByEmail!);
            var response = new AuthResponse
            {
                Id = userByEmail!.Id,
                Name = userByEmail!.Name,
                LastName = userByEmail!.LastName,
                Email = userByEmail!.Email,
                UserName = userByEmail.Name,
                Phone = userByEmail.PhoneNumber,
                Avatar = userByEmail.AvatarUrl,
                Token = _authService.CreateToken(userByEmail, roles),
                Roles = roles
            };
            return response;

        }
    }
}
