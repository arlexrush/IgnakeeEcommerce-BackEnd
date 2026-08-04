using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Reviews.Command.CreateReview
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewValidator()
        {
            RuleFor(r => r.Name).NotNull().WithMessage("The Name can´t be null");
            RuleFor(r => r.Comment).NotNull().WithMessage("The Comment can´t be null");
            RuleFor(r => r.Rating).NotEmpty().WithMessage("The Rating can´t be empty");

        }
    }
}
