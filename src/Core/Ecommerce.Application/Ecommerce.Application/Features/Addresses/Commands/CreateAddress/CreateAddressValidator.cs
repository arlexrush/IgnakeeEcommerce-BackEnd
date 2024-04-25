using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Addresses.Commands.CreateAddress
{
    public class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressValidator()
        {
            RuleFor(x => x.Address).NotEmpty().WithMessage("Can´t be empty");
            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Can´t be empty");
            RuleFor(x => x.City).NotEmpty().WithMessage("Can´t be empty");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Can´t be empty");
            RuleFor(x => x.Region).NotEmpty().WithMessage("Can´t be empty");
        }
    }
}
