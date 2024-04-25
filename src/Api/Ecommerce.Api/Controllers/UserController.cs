using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Auths.Roles.Queries.GetRoles;
using Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;
using Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser;
using Ecommerce.Application.Features.Auths.Users.Commands.ResetPasswordByToken;
using Ecommerce.Application.Features.Auths.Users.Commands.ResetPasswprd;
using Ecommerce.Application.Features.Auths.Users.Commands.SendPassword;
using Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminStatusUser;
using Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUser;
using Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser;
using Ecommerce.Application.Features.Auths.Users.Queries.GetUserById;
using Ecommerce.Application.Features.Auths.Users.Queries.GetUserByToken;
using Ecommerce.Application.Features.Auths.Users.Queries.GetUserByUserName;
using Ecommerce.Application.Features.Auths.Users.Queries.PaginationUsers;
using Ecommerce.Application.Features.Auths.Users.Vms;
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

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController:ControllerBase
    {
        private IMediator? _mediator;
        private IManageImageService? _manageImageService;

        public UserController(IMediator? mediator, IManageImageService? manageImageService)
        {
            _mediator = mediator;
            _manageImageService = manageImageService;
        }

        [AllowAnonymous]
        [HttpPost("login", Name ="login")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginUserCommand request)
        {
            var response = await _mediator!.Send(request);
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpPost("register", Name = "Register")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> Register([FromForm] RegisterUserCommand request)
        {
            if(request.ImageUser is not null)
            {
                
                var resultImage = await _manageImageService!.UploadImage(new ImageData
                {
                     ImageStream=request.ImageUser!.OpenReadStream(),
                     Name=request.ImageUser.Name,
                });

                request.ImageUserId = resultImage.PublicId;
                request.ImageUserUrl = resultImage.Url;
            }

            var response= await _mediator!.Send(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("forgotPassword", Name ="ForgotPassword")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> ForgotPassword([FromBody] SendPasswordCommand request)
        {
            var email=request.Email;
            var response=await _mediator!.Send(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("resetPassword", Name = "ResetPassword")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> ResetPassword([FromBody] ResetPasswordByTokenCommand request)
        {
            var response=await _mediator!.Send(request);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("updatePassword", Name = "UpdatePassword")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Unit>> UpdatePassword([FromBody] ResetPasswordCommand request)
        {
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("updateUser", Name = "UpdateUser")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> UpdateUser([FromForm] UpdateUserCommand request)
        {
            if(request.Photo is not null)
            {
                var resultImage = await _manageImageService!.UploadImage(new ImageData
                {
                    ImageStream = request.Photo!.OpenReadStream(),
                    Name = request.Photo.Name,
                });

                request.PhotoId = resultImage.PublicId;
                request.PhotoUrl= resultImage.Url;
            }

            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        [Authorize(Roles =Role.ADMIN)]
        [HttpPut("updateAdminUser", Name ="UpdateAdminUser")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> UpdateAdminUser([FromBody] UpdateAdminUserCommand request)
        {
            var response= await _mediator!.Send(request);
            return Ok(response);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpPut("updateAdminStatusUser", Name = "UpdateAdminStatusUser")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> UpdateAdminStatusUser([FromBody] UpdateAdminStatusUserCommand request)
        {
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> GetUserById(string id)
        {
            var request=new GetUserByIdQuery(id);
            var response = await _mediator!.Send(request);
            return Ok(response);
        }


        [Authorize]
        [HttpGet("", Name = "CurrentUser")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> CurrentUser()
        {
            var request = new GetUserByTokenQuery();
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("userName/{userName}", Name = "GetUserByUserName")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> GetUserByUserName(string userName)
        {
            var request = new GetUserByUserNameQuery(userName);
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("PaginationAdmin", Name = "PaginationAdmin")]
        [ProducesResponseType(typeof(PaginationVm<User>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<User>>> PaginationAdmin([FromQuery] PaginationUsersQuery paginationUsersQuery)
        {
            
            var PaginationProducts = await _mediator!.Send(paginationUsersQuery);
            return Ok(PaginationProducts);

        }

        [AllowAnonymous]
        [HttpGet("getRolesList", Name = "GetRolesList")]
        [ProducesResponseType(typeof(List<string>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<string>>> GetRolesList()
        {
            var query = new GetRolesQuery();
            var response= await _mediator!.Send(query);
            return Ok(response);
        }

       

    }
}
