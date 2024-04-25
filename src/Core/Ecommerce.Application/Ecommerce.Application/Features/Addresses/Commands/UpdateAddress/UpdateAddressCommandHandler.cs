using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, ShippingAddressVm>
    {
        private readonly IAuthService? _authService;
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public UpdateAddressCommandHandler(IAuthService? authService, IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ShippingAddressVm> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            //var addressRecord = await _unitOfWork!.Repository<Address>().GetEntityAsync(x => x.UserName == _authService!.GetSessionUser(), null, false);
            var addressTarget = await _unitOfWork!.Repository<Address>().GetByIdAsync(request.Id);           
            if (addressTarget is not null)
            {
                addressTarget!.UserName = _authService!.GetSessionUser();
                addressTarget.City = request.City;
                addressTarget.Region = request.Region;
                addressTarget.PostalCode = request.PostalCode;
                addressTarget.Country = request.Country;
                addressTarget.UserAddress = request.Address;
                addressTarget.LastModifiedBy = addressTarget.UserName;
                addressTarget.LastModifiedDate = DateTime.UtcNow;
                var addressUpdated=await _unitOfWork.Repository<Address>().UpdateAsync(addressTarget);

            }
            else
            {
                throw new InvalidOperationException("Not found Address to update");
            }           

            var response = _mapper!.Map<ShippingAddressVm>(addressTarget);

            return response;
        }
    }
}
