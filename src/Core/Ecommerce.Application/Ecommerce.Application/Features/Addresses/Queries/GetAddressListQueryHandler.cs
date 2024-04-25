using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Addresses.Queries
{
    public class GetAddressListQueryHandler : IRequestHandler<GetAddressListQuery, IReadOnlyList<ShippingAddressVm>>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IAuthService? _authService;
        private readonly IMapper? _mapper;
        private readonly UserManager<User>? _userManager;

        public GetAddressListQueryHandler(IUnitOfWork? unitOfWork, IAuthService? authService, IMapper? mapper, UserManager<User>? userManager)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<ShippingAddressVm>> Handle(GetAddressListQuery request, CancellationToken cancellationToken)
        {
            var address=await _unitOfWork!.Repository<Address>().GetAsync(x=>x.UserName!.Equals(_authService!.GetSessionUser()));
            var response = _mapper!.Map<IReadOnlyList<ShippingAddressVm>>(address);
            return response;
        }
    }
}
