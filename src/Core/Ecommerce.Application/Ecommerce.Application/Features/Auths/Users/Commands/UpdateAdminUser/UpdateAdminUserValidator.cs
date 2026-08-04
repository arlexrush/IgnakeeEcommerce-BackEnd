using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUser
{
    public class UpdateAdminUserValidator : AbstractValidator<UpdateAdminUserCommand>
    {
        public UpdateAdminUserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("The Name Can´t be empty");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("The Last Name Can´t be empty");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("The Phone Can´t be empty");
        }
    }
}
