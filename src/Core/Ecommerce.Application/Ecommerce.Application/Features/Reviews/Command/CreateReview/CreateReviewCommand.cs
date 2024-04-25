using Ecommerce.Application.Features.Products.Queries.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Reviews.Command.CreateReview
{
    public class CreateReviewCommand:IRequest<ReviewVm>
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }

    }
}
