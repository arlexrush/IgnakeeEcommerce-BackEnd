using Ecommerce.Application.Features.ShoppingCarts.Commands.AddItemShoppingcart;
using Ecommerce.Application.Features.ShoppingCarts.Commands.CreateShoppingCart;
using Ecommerce.Application.Features.ShoppingCarts.Commands.DeleteItemShoppingCart;
using Ecommerce.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AddItemShoppingCartCommand = Ecommerce.Application.Features.ShoppingCarts.Commands.AddItemShoppingcart.AddItemShoppingCartCommand;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingCartController : ControllerBase
    {

        private readonly IMediator? _mediator;

        public ShoppingCartController(IMediator? mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetShoppingCart")]
        [ProducesResponseType(typeof(ShoppingCartVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartVm>> GetShoppingCart(Guid id)
        {
            var shoppingCartId = id == Guid.Empty ? Guid.NewGuid() : id;
            var request = new Application.Features.ShoppingCarts.Queries.AddItemShoppingCartCommand(shoppingCartId);
            var response = await _mediator!.Send(request);
            return response;
        }


        [AllowAnonymous]
        [HttpPut("{id}", Name = "UpdateShoppingCart")]
        [ProducesResponseType(typeof(ShoppingCartVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartVm>> UpdateShoppingCart(Guid id, UpdateShoppingCartCommand request)
        {
            request.ShoppingCartId = id;
            var response = await _mediator!.Send(request);
            return response;
        }

        [AllowAnonymous]
        [HttpPut("addItem/{id}", Name = "AddItemShoppingCart")]
        [ProducesResponseType(typeof(ShoppingCartVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartVm>> AddItemShoppingCart(Guid id, AddItemShoppingCartCommand request)
        {
            request.ShoppingCartId = id;
            var response = await _mediator!.Send(request);
            return response;
        }

        [AllowAnonymous]
        [HttpDelete("deleteItem/{id}", Name = "deleteItemShoppingCart")]
        [ProducesResponseType(typeof(ShoppingCartVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartVm>> deleteItemShoppingCart(int id)
        {
            DeleteItemShoppingCartCommand request = new DeleteItemShoppingCartCommand() { Id = id };
            var response = await _mediator!.Send(request);
            return response;
        }

        [AllowAnonymous]
        [HttpPost("createShoppingCart", Name = "CreateShoppingCart")]
        [ProducesResponseType(typeof(ShoppingCartVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartVm>> CreateShoppingCart()
        {
            CreateShoppingCartCommand request = new CreateShoppingCartCommand();
            var response = await _mediator!.Send(request);
            return response;
        }

    }
}
