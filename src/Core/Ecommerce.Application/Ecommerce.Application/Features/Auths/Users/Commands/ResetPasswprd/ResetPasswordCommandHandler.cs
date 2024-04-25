using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPasswprd
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {

        private readonly UserManager<User>? _userManager;
        private readonly IAuthService? _authService;

        public ResetPasswordCommandHandler(UserManager<User>? userManager, IAuthService? authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var updateUser= await _userManager!.FindByNameAsync(_authService!.GetSessionUser());

            if (updateUser is null)
            {
                throw new BadRequestException("The User don´t Exist");
            }
            var resultValidateOldPassword = _userManager.PasswordHasher.VerifyHashedPassword(updateUser, updateUser.PasswordHash!, request.OldPassword!);

            if(!(resultValidateOldPassword== PasswordVerificationResult.Success))
            {
                throw new BadRequestException("the enter password is incorrect");
            }

            var hashedNewPassword = _userManager.PasswordHasher.HashPassword(updateUser, request.NewPassword!);
            updateUser.PasswordHash = hashedNewPassword;
            var result= await _userManager.UpdateAsync(updateUser);
            if(!result.Succeeded)
            {
                throw new Exception("It Can´t to update your password");
            }

            return Unit.Value;
        }
    }
}
