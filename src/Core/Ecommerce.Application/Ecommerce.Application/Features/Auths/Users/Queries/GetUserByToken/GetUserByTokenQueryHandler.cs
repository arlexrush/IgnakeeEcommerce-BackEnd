using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserByToken
{
    public class GetUserByTokenQueryHandler : IRequestHandler<GetUserByTokenQuery, AuthResponse>
    {
        private readonly UserManager<User>? _userManager;
        private readonly IAuthService? _authService;
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public GetUserByTokenQueryHandler(UserManager<User>? userManager, 
                                            IAuthService? authService, 
                                            IUnitOfWork? unitOfWork, 
                                            IMapper? mapper)
        {
            _userManager = userManager;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuthResponse> Handle(GetUserByTokenQuery request, CancellationToken cancellationToken)
        {
            var user=await _userManager!.FindByNameAsync(_authService!.GetSessionUser());
            if (user is null)
            {
                throw new Exception("The User haven´t enough Credentials, No Authenticade");
            }
            if(!user.IsActive)
            {
                throw new Exception("The User is Inactive");
            }

            var shippingAddress=await _unitOfWork!.Repository<Address>().GetEntityAsync(x=>x.UserName==user.UserName);
            var roles = await _userManager.GetRolesAsync(user);
            var authResponse = new AuthResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.UserName,
                LastName = user.UserName,
                Email = user.UserName,
                Phone = user.UserName,
                Avatar = user.AvatarUrl,
                Roles = roles,
                ShippingAddress = _mapper!.Map<ShippingAddressVm>(shippingAddress),
                Token = _authService.CreateToken(user, roles)
            };

            return authResponse;
        }
    }
}
