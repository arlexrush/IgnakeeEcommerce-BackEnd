using Ecommerce.Application.Features.Addresses.Commands.CreateAddress;
using Ecommerce.Application.Features.Addresses.Queries;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Taxes.Commands.CreateTaxCommand;
using Ecommerce.Application.Features.Taxes.Commands.DeleteTax;
using Ecommerce.Application.Features.Taxes.Commands.UpdateTax;
using Ecommerce.Application.Features.Taxes.Queries.GetTaxesByCountry;
using Ecommerce.Application.Features.Taxes.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TaxController : ControllerBase
    {
        private readonly IMediator? _mediator;

        public TaxController(IMediator? mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("listTaxesByCountry/{id}", Name = "GetTaxesByCountryList")]
        [ProducesResponseType(typeof(IReadOnlyList<TaxVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<TaxVm>>> GetTaxesByCountryList(int id)
        {
            var query = new GetTaxesByCountryQuery() { CountryId = id };
            IReadOnlyList<TaxVm> responseTaxes = await _mediator!.Send(query);
            return Ok(responseTaxes);
        }

        [Authorize]
        [HttpPost("createTax", Name = "CreateTax")]
        [ProducesResponseType(typeof(TaxVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TaxVm>> CreateAddress([FromForm] CreateTaxCommand request)
        {

            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("updateTax", Name = "UpdateTax")]
        [ProducesResponseType(typeof(TaxVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TaxVm>> UpdateTax([FromBody] UpdateTaxCommand request)
        {

            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("deleteTax/{id}", Name = "DeleteTax")]
        [ProducesResponseType(typeof(TaxVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TaxVm>> DeleteTax(int id)
        {
            var request = new DeleteTaxCommand() { Id = id };
            var response = await _mediator!.Send(request);
            return Ok(response);
        }
    }
}
