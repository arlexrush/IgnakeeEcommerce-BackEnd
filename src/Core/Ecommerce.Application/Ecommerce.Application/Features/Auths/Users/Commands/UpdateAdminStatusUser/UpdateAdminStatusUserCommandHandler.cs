using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminStatusUser
{
    public class UpdateAdminStatusUserCommandHandler : IRequestHandler<UpdateAdminStatusUserCommand, User>
    {
        private readonly UserManager<User>? _userManager;

        public UpdateAdminStatusUserCommandHandler(UserManager<User>? userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> Handle(UpdateAdminStatusUserCommand request, CancellationToken cancellationToken)
        {
            var updateUser= await _userManager!.FindByIdAsync(request.Id!);
            if (updateUser is null)
            {
                throw new BadRequestException("This user don´t exist, please try again with other user");
            }

            updateUser.IsActive= !updateUser.IsActive;
            var result = await _userManager.UpdateAsync(updateUser);
            if (!result.Succeeded)
            {
                throw new Exception("it can´t update record");
            }

            return updateUser;


        }
    }
}
