using Ecommerce.Application.Features.Addresses.Commands;
using Ecommerce.Application.Features.Addresses.Commands.CreateAddress;
using Ecommerce.Application.Features.Addresses.Commands.DeleteAddress;
using Ecommerce.Application.Features.Addresses.Commands.UpdateAddress;
using Ecommerce.Application.Features.Addresses.Queries;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Models.Authorization;
using Ecommerce.Application.Models.ImageMangement;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AddressController:ControllerBase
    {
        private readonly IMediator? _mediator;

        public AddressController(IMediator? mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("listAddress", Name = "GetAddressList")]
        [ProducesResponseType(typeof(IReadOnlyList<ShippingAddressVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<ShippingAddressVm>>> GetAddressList()
        {
            var query = new GetAddressListQuery();
            IReadOnlyList<ShippingAddressVm> responseProductList = await _mediator!.Send(query);
            return Ok(responseProductList);
        }

        [Authorize]
        [HttpPost("createAddress", Name = "CreateAddress")]
        [ProducesResponseType(typeof(ShippingAddressVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingAddressVm>> CreateAddress([FromForm] CreateAddressCommand request)
        {
            
            var response = await _mediator!.Send(request);
            return Ok(response);
        }


        [Authorize]
        [HttpPut("updateAddress", Name = "UpdateAddress")]
        [ProducesResponseType(typeof(ShippingAddressVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingAddressVm>> UpdateAddress([FromBody] UpdateAddressCommand request)
        {

            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("deleteAddress/{id}", Name = "DeleteAddress")]
        [ProducesResponseType(typeof(ShippingAddressVm),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingAddressVm>> DeleteAddress(int id)
        {
            var request=new DeleteAddressCommand() { Id=id };
            var response= await _mediator!.Send(request);
            return Ok(response);
        }
    }
}
