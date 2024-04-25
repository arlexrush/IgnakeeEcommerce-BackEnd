using Ecommerce.Application.Features.Products.Queries.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Reviews.Command.DeleteReview
{
    public class DeleteReviewCommand:IRequest<Unit>
    {
        public int ReviewId { get; set; }

        public DeleteReviewCommand(int reviewId)
        {
            ReviewId = reviewId==0? throw new ArgumentException(nameof(reviewId)):reviewId;
        }
    }
}
