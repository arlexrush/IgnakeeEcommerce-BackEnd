using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email can´t be null");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password can´t be null");
        }
    }
}
