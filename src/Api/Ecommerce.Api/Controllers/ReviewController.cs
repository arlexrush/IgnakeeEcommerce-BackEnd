using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Reviews.Command.CreateReview;
using Ecommerce.Application.Features.Reviews.Command.DeleteReview;
using Ecommerce.Application.Features.Reviews.Queries.PaginationReview;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Models.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReviewController:ControllerBase
    {
        private readonly IMediator? _mediator;

        public ReviewController(IMediator? mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("createReview", Name = "CreateReview")]
        [ProducesResponseType(typeof(ReviewVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ReviewVm>> CreateReview([FromBody] CreateReviewCommand request)
        {
            var response=await _mediator!.Send(request);
            return Ok(response);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpDelete("deleteReview/{id}", Name = "DeleteReview")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Unit>> DeleteReview(int id)
        {
            var request = new DeleteReviewCommand(id);
            var response =await _mediator!.Send(request);
            return response;
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("paginationReviews", Name = "PaginationReviews")]
        [ProducesResponseType(typeof(PaginationVm<ReviewVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<ReviewVm>>> PaginationReviews([FromQuery] PaginationReviewQuery request)
        {
            var response=await _mediator!.Send(request);
            return response;
        }

    }
}
