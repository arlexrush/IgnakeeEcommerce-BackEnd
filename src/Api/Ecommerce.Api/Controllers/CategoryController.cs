using Ecommerce.Application.Features.Categories.Queries.GetCategoryList;
using Ecommerce.Application.Features.Countries.Queries.GetCountryList;
using Ecommerce.Application.Features.Countries.Queries.Vm;
using Ecommerce.Application.Features.Products.Queries.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController:ControllerBase
    {
        private readonly IMediator? _mediator;

        public CategoryController(IMediator? mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("getCategories", Name = "GetCategories")]
        [ProducesResponseType(typeof(IReadOnlyCollection<CategoryVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyCollection<CategoryVm>>> GetCategories()
        {
            var query = new GetCategoryListQuery();

            var response = await _mediator!.Send(query);
            return Ok(response);
        }
    }
}
