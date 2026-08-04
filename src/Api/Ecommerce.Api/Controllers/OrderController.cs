using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Commands.CreateAddress;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Orders.Commands.CreateOrder;
using Ecommerce.Application.Features.Orders.Commands.UpdateOrder;
using Ecommerce.Application.Features.Orders.Queries.GetOrderById;
using Ecommerce.Application.Features.Orders.Queries.PaginationOrders;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Features.Products.Queries.Vms;
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
    public class OrderController : ControllerBase
    {
        private IMediator _mediator;
        private readonly IAuthService _authService;

        public OrderController(IMediator mediator, IAuthService authService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [Authorize]
        [HttpPost("createOrderAddress", Name = "CreateOrderAddress")]
        [ProducesResponseType(typeof(ProductVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingAddressVm>> CreateOrderAddress([FromForm] CreateAddressCommand request)
        {
            var response = await _mediator.Send(request);
            return response;
        }

        [Authorize]
        [HttpPost(Name = "CreateOrder")]
        [ProducesResponseType(typeof(OrderVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderVm>> CreateOrder([FromBody] CreateOrderCommand request)
        {
            var response = await _mediator.Send(request);
            return response;
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(typeof(OrderVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderVm>> UpdateOrder([FromBody] UpdateOrderCommand request)
        {
            var response = await _mediator.Send(request);
            return response;
        }

        [AllowAnonymous]
        [HttpGet("getOrderById/{id}", Name = "GetOrderById")]
        [ProducesResponseType(typeof(OrderVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderVm>> GetOrderById(int id)
        {
            var request = new GetOrderByIdQuery(id);
            var response = await _mediator.Send(request);
            return response;
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("pagination", Name = "PaginationOrders")]
        [ProducesResponseType(typeof(PaginationVm<OrderVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<OrderVm>>> PaginationOrders([FromQuery] PaginationOrdersQuery paginationOrdersQuery)
        {
            var response = await _mediator.Send(paginationOrdersQuery);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("paginationByUserName", Name = "PaginationOrderByUserName")]
        [ProducesResponseType(typeof(PaginationVm<OrderVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<OrderVm>>> PaginationOrderByUserName([FromQuery] PaginationOrdersQuery paginationOrdersQuery)
        {
            paginationOrdersQuery.UserName = _authService.GetSessionUser();
            var response = await _mediator.Send(paginationOrdersQuery);
            return Ok(response);
        }

    }
}
