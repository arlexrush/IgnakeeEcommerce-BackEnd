using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(UserManager<User> userManager, 
                                            SignInManager<User> signInManager, 
                                            RoleManager<IdentityRole> roleManager, 
                                            IAuthService authService, 
                                            IMapper mapper, 
                                            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _authService = authService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existUserByEmail = await _userManager.FindByEmailAsync(request.Email!) is null ? false : true;
            if (existUserByEmail)
            {
                throw new BadRequestException($"This Email: {request.Email} Exist, please enter new email");
            }
            var existUserByUserName = await _userManager.FindByEmailAsync(request.UserName!) is null ? false : true;
            if (existUserByUserName)
            {
                throw new BadRequestException($"This UserName: {request.UserName} Exist, please enter new UserName");
            }
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Name = request.Name,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                AvatarUrl = request.ImageUserUrl,
            };
            var result =    await _userManager.CreateAsync(user, request.Password!);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, AppRole.GeneryUser);
                var roles=await _userManager.GetRolesAsync(user);
                var authResponse = new AuthResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Name = user.Name,
                    LastName = user.LastName,
                    Phone = user.PhoneNumber,
                    Avatar = user.AvatarUrl,
                    Roles = roles,
                    Token = _authService.CreateToken(user, roles),
                };

                return authResponse;

            }
            throw new Exception("We don´t create user, please contact Administrator");


        }
    }
}
