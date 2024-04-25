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
    public class CountryController:ControllerBase
    {
        private readonly IMediator? _mediator;

        public CountryController(IMediator? mediator)
        {
            _mediator = mediator;
        }


        [AllowAnonymous]
        [HttpGet("getCountries", Name ="GetCountries")]
        [ProducesResponseType(typeof(IReadOnlyCollection<CountryVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyCollection<CountryVm>>> GetCountries()
        {
            var query = new GetCountryListQuery();

            var response = await _mediator!.Send(query);
            return Ok(response);
        }

    }
}
