using Ecommerce.Application.Features.Inventory.Queries.GetInventoryProductByCode;
using Ecommerce.Application.Features.Inventory.Queries.PaginationInventoryProducts;
using Ecommerce.Application.Features.Inventory.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Models.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = $"{Role.ADMIN},{Role.INVENTORY_READER}")]
    [Route("api/v1/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{productCode}", Name = "GetInventoryProductByCode")]
        [ProducesResponseType(typeof(InventoryProductVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<InventoryProductVm>> GetInventoryProductByCode(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode))
            {
                return BadRequest("Product code is required.");
            }

            var product = await _mediator.Send(new GetInventoryProductByCodeQuery(productCode));
            return Ok(product);
        }

        [HttpGet(Name = "PaginationInventoryProducts")]
        [ProducesResponseType(typeof(PaginationVm<InventoryProductVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PaginationVm<InventoryProductVm>>> PaginationInventoryProducts([FromQuery] PaginationInventoryProductsQuery request)
        {
            if (request.PageIndex is < 1)
            {
                return BadRequest("PageIndex must be greater than zero.");
            }

            if (request.PageSize < 1)
            {
                return BadRequest("PageSize must be greater than zero.");
            }

            if (request.CategoryId is <= 0)
            {
                return BadRequest("CategoryId must be greater than zero.");
            }

            var products = await _mediator.Send(request);
            return Ok(products);
        }
    }
}
