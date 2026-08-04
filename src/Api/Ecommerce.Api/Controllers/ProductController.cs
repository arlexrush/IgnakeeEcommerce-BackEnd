using CloudinaryDotNet.Actions;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Products.Commands.AddDimensionsToProduct;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Commands.DeleteProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateProduct;
using Ecommerce.Application.Features.Products.Commands.Vms;
using Ecommerce.Application.Features.Products.Queries.GetProductById;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Features.Products.Queries.PaginationProducts;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Models.Authorization;
using Ecommerce.Application.Models.ImageMangement;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Role = Ecommerce.Application.Models.Authorization.Role;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private IMediator _mediator;
        private readonly IManageImageService _imageService;


        public ProductController(IMediator mediator, IManageImageService imageService)
        {
            _mediator = mediator;
            _imageService = imageService;

        }

        [AllowAnonymous]
        [HttpGet("list", Name = "GetProductList")]
        [ProducesResponseType(typeof(IReadOnlyList<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProductList()
        {
            var query = new GetProductListQuery();
            IReadOnlyList<Product> responseProductList = await _mediator.Send(query);
            return Ok(responseProductList);
        }

        [AllowAnonymous]
        [HttpGet("pagination", Name = "PaginationProduct")]
        [ProducesResponseType(typeof(PaginationVm<ProductVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<ProductVm>>> PaginationProduct([FromQuery] PaginationProductsQuery paginationProductsQuery)
        {
            try
            {
                paginationProductsQuery.Status = ProductStatus.Active;
                var PaginationProducts = await _mediator.Send(paginationProductsQuery);
                return Ok(PaginationProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("paginationAdmin", Name = "PaginationProductAdmin")]
        [ProducesResponseType(typeof(PaginationVm<ProductVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<ProductVm>>> PaginationProductAdmin([FromQuery] PaginationProductsQuery paginationProductsQuery)
        {
            var PaginationProducts = await _mediator.Send(paginationProductsQuery);
            return Ok(PaginationProducts);

        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductVm>> GetProductById(int id)
        {
            var getProductById = new GetProductByIdQuery(id);
            var productById = await _mediator.Send(getProductById);
            return Ok(productById);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpPost("createProduct", Name = "CreateProduct")]
        [ProducesResponseType(typeof(ProductVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductVm>> CreateProduct([FromForm] CreateProductCommand request)
        {
            var listImagesUrls = new List<CreateProductImageCommand>();
            if (request.ProductRequestImages is not null)
            {
                foreach (var image in request.ProductRequestImages)
                {
                    var imageClient = await _imageService.UploadImage(new ImageData
                    {
                        ImageStream = image.OpenReadStream(),
                        Name = image.Name,
                    });

                    var imageCommand = new CreateProductImageCommand
                    {
                        PublicCode = imageClient.PublicId,
                        Url = imageClient.Url
                    };

                    listImagesUrls.Add(imageCommand);
                }
                request.ImageUrls = listImagesUrls;

            }
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpPut("updateProduct", Name = "UpdateProduct")]
        [ProducesResponseType(typeof(ProductVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductVm>> UpdateProduct([FromForm] UpdateProductCommand request)
        {
            var listImagesUrls = new List<CreateProductImageCommand>();
            if (request.ProductRequestImages is not null)
            {
                foreach (var image in request.ProductRequestImages)
                {
                    var imageClient = await _imageService.UploadImage(new ImageData
                    {
                        ImageStream = image.OpenReadStream(),
                        Name = image.Name,
                    });

                    var imageCommand = new CreateProductImageCommand
                    {
                        PublicCode = imageClient.PublicId,
                        Url = imageClient.Url
                    };

                    listImagesUrls.Add(imageCommand);
                }
                request.ImageUrls = listImagesUrls;

            }
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpDelete("status/{id}", Name = "UpdateStatusProduct")]
        [ProducesResponseType(typeof(ProductVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductVm>> UpdateStatusProduct(int id)
        {
            var request = new DeleteProductCommand(id);

            var response = await _mediator.Send(request);
            return Ok(response);
        }


        [Authorize(Roles = Role.ADMIN)]
        [HttpPost("createProductDimension", Name = "CreateProductDimension")]
        [ProducesResponseType(typeof(ProductDimensionVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductDimensionVm>> CreateProductDimension([FromForm] AddDimensionsToProductCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
