using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPasswordByToken
{
    public class ResetPasswordByTokenCommandHandler : IRequestHandler<ResetPasswordByTokenCommand, string>
    {
        private readonly UserManager<User>? _userManager;

        public ResetPasswordByTokenCommandHandler(UserManager<User>? userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Handle(ResetPasswordByTokenCommand request, CancellationToken cancellationToken)
        {
            if(!string.Equals(request.Password, request.ConfirmPassword))
            {
                throw new BadRequestException("Your new Passwoord don´t has been confirmed successfully");
            }

            var userToUpdate = await _userManager!.FindByEmailAsync(request.Email!);
            if (userToUpdate == null)
            {
                throw new BadRequestException("The User don´t exist or your Email don´t exist");
            }

            var token=Convert.FromBase64String(request.Token!);
            var tokenResult= Encoding.UTF8.GetString(token);
            var resetResult= await _userManager.ResetPasswordAsync(userToUpdate, tokenResult, request.Password!);
            if(!resetResult.Succeeded)
            {
                throw new Exception("Can´t to reset your password");
            }

            return "it has updated your password";
        }
    }
}
