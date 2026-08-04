using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Reviews.Queries.PaginationReview
{
    public class PaginationReviewQuery : PaginationBaseQuery, IRequest<PaginationVm<ReviewVm>>
    {
        public int? ProductId { get; set; }
    }
}
