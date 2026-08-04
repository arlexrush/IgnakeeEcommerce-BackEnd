using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("The name of Product can´t be empty")
                .MaximumLength(100)
                .WithMessage("The name of Product can´t be more than 100 character");

            RuleFor(x => x.ProductDescription)
                .NotEmpty()
                .WithMessage("The Description can´t be null");

            RuleFor(x => x.Stock)
                .NotEmpty()
                .WithMessage("The Stock can´t be null");

            RuleFor(x => x.ProductPrice)
                .NotEmpty()
                .WithMessage("The price of the product can´t be empty or null");
        }
    }
}
