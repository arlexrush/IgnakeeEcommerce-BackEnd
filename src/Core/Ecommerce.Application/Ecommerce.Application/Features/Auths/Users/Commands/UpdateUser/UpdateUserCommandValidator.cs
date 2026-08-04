using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("The Name can´t be empty");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("The LastName can´t be empty");
        }
    }
}
