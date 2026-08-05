using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Contracts.Identity;
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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private IMediator? _mediator;
        private IManageImageService? _manageImageService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public UserController(IMediator? mediator,
                              IManageImageService? manageImageService,
                              SignInManager<User> signInManager,
                              UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IAuthService authService,
                              IConfiguration configuration)
        {
            _mediator = mediator;
            _manageImageService = manageImageService;
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        /// <summary>
        /// Responsible for authenticating a user and returning an authentication response containing user details and a JWT token.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <returns>An authentication response containing user details and a JWT token.</returns>
        [AllowAnonymous]
        [HttpPost("login", Name = "login")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginUserCommand request)
        {
            var response = await _mediator!.Send(request);
            return Ok(response);
        }


        /// <summary>
        /// Responsable for initiating the Google authentication process. It checks if Google authentication is configured, and if so, it generates a callback URL and challenges the user to authenticate with Google.
        /// </summary>
        /// <returns>An IActionResult that redirects the user to the Google authentication page.</returns>
        [AllowAnonymous]
        [HttpGet("external/google", Name = "GoogleLogin")]
        public IActionResult GoogleLogin()
        {
            if (!GoogleAuthenticationIsConfigured())
            {
                return Problem("Google authentication is not configured.", statusCode: StatusCodes.Status503ServiceUnavailable);
            }

            var callbackPath = Url.Action(nameof(GoogleCallback), "User")!;
            var publicBaseUrl = _configuration["Authentication:PublicBaseUrl"];
            var callbackUrl = string.IsNullOrWhiteSpace(publicBaseUrl)
                ? Url.Action(nameof(GoogleCallback), "User", null, Request.Scheme)
                : $"{publicBaseUrl.TrimEnd('/')}/{callbackPath.TrimStart('/')}";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", callbackUrl!);
            return Challenge(properties, "Google");
        }

        /// <summary>
        /// Responsable for handling the callback from Google after the user has authenticated. It retrieves the external login information, checks if the email is verified, and either logs in the user or creates a new user account if necessary. Finally, it returns an authentication response containing user details and a JWT token.
        /// </summary>
        /// <returns>An authentication response containing user details and a JWT token.</returns>
        [AllowAnonymous]
        [HttpGet("external/google/callback", Name = "GoogleCallback")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> GoogleCallback()
        {
            if (!GoogleAuthenticationIsConfigured())
            {
                return Problem("Google authentication is not configured.", statusCode: StatusCodes.Status503ServiceUnavailable);
            }

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo is null)
            {
                return Unauthorized("The Google authentication result is missing or expired.");
            }

            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var emailVerified = externalLoginInfo.Principal.FindFirstValue("email_verified");
            if (string.IsNullOrWhiteSpace(email) || !bool.TryParse(emailVerified, out var isEmailVerified) || !isEmailVerified)
            {
                return BadRequest("Google did not provide a verified email address.");
            }

            var user = await _userManager.FindByLoginAsync(
                externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey);

            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user is null)
                {
                    user = new User
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        Name = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
                        LastName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname),
                        IsActive = true
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        return BadRequest("The ecommerce profile could not be created.");
                    }

                    if (!await _roleManager.RoleExistsAsync(AppRole.GeneryUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(AppRole.GeneryUser));
                    }

                    var roleResult = await _userManager.AddToRoleAsync(user, AppRole.GeneryUser);
                    if (!roleResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(user);
                        return BadRequest("The ecommerce profile role could not be assigned.");
                    }
                }

                var loginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
                if (!loginResult.Succeeded)
                {
                    return BadRequest("The Google account could not be linked to the ecommerce profile.");
                }
            }

            if (!user.IsActive)
            {
                return Unauthorized("The ecommerce profile is inactive.");
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new AuthResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                Roles = roles,
                Avatar = user.AvatarUrl,
                Token = _authService.CreateToken(user, roles)
            });
        }

        /// <summary>
        /// Responsible for registering a new user. It handles the registration request, including optional image upload, and returns an authentication response containing user details and a JWT token.
        /// </summary>
        /// <param name="request">The registration request containing user details and an optional profile image.</param>
        /// <returns>An authentication response containing user details and a JWT token.</returns>
        [AllowAnonymous]
        [HttpPost("register", Name = "Register")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> Register([FromForm] RegisterUserCommand request)
        {
            if (request.ImageUser is not null)
            {

                var resultImage = await _manageImageService!.UploadImage(new ImageData
                {
                    ImageStream = request.ImageUser!.OpenReadStream(),
                    Name = request.ImageUser.Name,
                });

                request.ImageUserId = resultImage.PublicId;
                request.ImageUserUrl = resultImage.Url;
            }

            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Responsible for checking if Google authentication is properly configured by verifying the presence of the ClientId and ClientSecret in the configuration settings.
        /// </summary>
        /// <returns>True if Google authentication is configured; otherwise, false.</returns>
        private bool GoogleAuthenticationIsConfigured()
        {
            return !string.IsNullOrWhiteSpace(_configuration["Authentication:Google:ClientId"]) &&
                !string.IsNullOrWhiteSpace(_configuration["Authentication:Google:ClientSecret"]);
        }

        /// <summary>
        /// Responsable for initiating the password reset process. It accepts a request containing the user's email address and sends a password reset email if the email is associated with an existing user account. The response indicates whether the password reset email was sent successfully.
        /// </summary>
        /// <param name="request">The request containing the user's email address.</param>
        /// <returns>A response indicating whether the password reset email was sent successfully.</returns>
        [AllowAnonymous]
        [HttpPost("forgotPassword", Name = "ForgotPassword")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> ForgotPassword([FromBody] SendPasswordCommand request)
        {
            var email = request.Email;
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Responsable for resetting the user's password using a token. It accepts a request containing the user's email, new password, and the reset token. If the token is valid and the password is successfully reset, it returns a success message; otherwise, it returns an error message.
        /// </summary>
        /// <param name="request">The request containing the user's email, new password, and the reset token.</param>
        /// <returns>A response indicating whether the password was successfully reset.</returns>
        [AllowAnonymous]
        [HttpPost("resetPassword", Name = "ResetPassword")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> ResetPassword([FromBody] ResetPasswordByTokenCommand request)
        {
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Responsable for updating the user's password. It accepts a request containing the user's old password and new password. If the old password is correct and the new password meets the requirements, it updates the password and returns a success response; otherwise, it returns an error response.
        /// </summary>
        /// <param name="request">The request containing the user's old password and new password.</param>
        /// <returns>A response indicating whether the password was successfully updated.</returns>
        [Authorize]
        [HttpPost("updatePassword", Name = "UpdatePassword")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Unit>> UpdatePassword([FromBody] ResetPasswordCommand request)
        {
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Responsable for updating the user's profile information, including optional profile image upload. It accepts a request containing the updated user details and returns an authentication response containing the updated user details and a JWT token.
        /// </summary>
        /// <param name="request">The request containing the updated user details and optional profile image.</param>
        /// <returns>A response containing the updated user details and a JWT token.</returns>
        [Authorize]
        [HttpPost("updateUser", Name = "UpdateUser")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> UpdateUser([FromForm] UpdateUserCommand request)
        {
            if (request.Photo is not null)
            {
                var resultImage = await _manageImageService!.UploadImage(new ImageData
                {
                    ImageStream = request.Photo!.OpenReadStream(),
                    Name = request.Photo.Name,
                });

                request.PhotoId = resultImage.PublicId;
                request.PhotoUrl = resultImage.Url;
            }

            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Responsable for updating an admin user's profile information. It accepts a request containing the updated admin user details and returns the updated user entity. This endpoint is restricted to users with the "ADMIN" role.   
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.ADMIN)]
        [HttpPut("updateAdminUser", Name = "UpdateAdminUser")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> UpdateAdminUser([FromBody] UpdateAdminUserCommand request)
        {
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Responsable for updating the admin status of a user. It accepts a request containing the user's ID and the new admin status, and returns the updated user entity. This endpoint is restricted to users with the "ADMIN" role.
        /// </summary>
        /// <param name="request">The request containing the user's ID and the new admin status.</param>
        /// <returns>The updated user entity.</returns>
        [Authorize(Roles = Role.ADMIN)]
        [HttpPut("updateAdminStatusUser", Name = "UpdateAdminStatusUser")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> UpdateAdminStatusUser([FromBody] UpdateAdminStatusUserCommand request)
        {
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Responsable for retrieving a user's details by their unique identifier. It accepts the user's ID as a parameter and returns an authentication response containing the user's details. This endpoint is restricted to users with the "ADMIN" role.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>An authentication response containing the user's details.</returns>
        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> GetUserById(string id)
        {
            var request = new GetUserByIdQuery(id);
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Responsable for retrieving the details of the currently authenticated user based on the provided JWT token. It returns an authentication response containing the user's details. This endpoint requires the user to be authenticated.
        /// </summary>
        /// <returns>An authentication response containing the user's details.</returns>
        [Authorize]
        [HttpGet("", Name = "CurrentUser")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> CurrentUser()
        {
            var request = new GetUserByTokenQuery();
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Responsable for retrieving a user's details by their username. It accepts the username as a parameter and returns an authentication response containing the user's details. This endpoint is restricted to users with the "ADMIN" role.
        /// </summary>
        /// <param name="userName">The username of the user.</param>
        /// <returns>An authentication response containing the user's details.</returns>
        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("userName/{userName}", Name = "GetUserByUserName")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthResponse>> GetUserByUserName(string userName)
        {
            var request = new GetUserByUserNameQuery(userName);
            var response = await _mediator!.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Responsable for retrieving a paginated list of users. It accepts pagination parameters and returns a paginated view model containing user data. This endpoint is restricted to users with the "ADMIN" role.
        /// </summary>
        /// <param name="paginationUsersQuery">The pagination parameters for retrieving users.</param>
        /// <returns>A paginated view model containing user data.</returns>
        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("PaginationAdmin", Name = "PaginationAdmin")]
        [ProducesResponseType(typeof(PaginationVm<User>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<User>>> PaginationAdmin([FromQuery] PaginationUsersQuery paginationUsersQuery)
        {

            var PaginationProducts = await _mediator!.Send(paginationUsersQuery);
            return Ok(PaginationProducts);

        }

        /// <summary>
        /// Responsable for retrieving a list of available roles in the system. It returns a list of role names. This endpoint is accessible to all users, including anonymous users.
        /// </summary>
        /// <returns>A list of role names.</returns>
        [AllowAnonymous]
        [HttpGet("getRolesList", Name = "GetRolesList")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<string>>> GetRolesList()
        {
            var query = new GetRolesQuery();
            var response = await _mediator!.Send(query);
            return Ok(response);
        }



    }
}
