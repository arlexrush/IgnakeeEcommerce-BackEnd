using Ecommerce.Application.Features.Products.Queries.GetInventoryProductByCode;
using Ecommerce.Application.Features.Products.Queries.GetInventoryProductCatalog;
using Ecommerce.Application.Features.Products.Queries.Vms.Inventory;
using Ecommerce.Application.Features.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    /// <summary>
    /// Read-only inventory surface for the IgnakeeAI.McpServer.Supplier service-to-service integration.
    /// All endpoints require authentication. Callers must hold the ADMIN or SUPPLIER_INTEGRATION role.
    /// No write/mutation capability is exposed here.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Policy = "SupplierIntegration")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns the inventory view for a single active product identified by its canonical ProductCode.
        /// </summary>
        /// <param name="productCode">The canonical product code (e.g. "P-001").</param>
        [HttpGet("product/{productCode}", Name = "GetInventoryProductByCode")]
        [ProducesResponseType(typeof(InventoryProductVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<InventoryProductVm>> GetByProductCode(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode))
            {
                return BadRequest("productCode is required.");
            }

            var query = new GetInventoryProductByCodeQuery(productCode);
            var result = await _mediator.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Returns a paginated list of active products for catalog synchronization.
        /// Supports optional text search and category filtering.
        /// </summary>
        [HttpGet("catalog", Name = "GetInventoryProductCatalog")]
        [ProducesResponseType(typeof(PaginationVm<InventoryProductVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PaginationVm<InventoryProductVm>>> GetCatalog(
            [FromQuery] GetInventoryProductCatalogQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
