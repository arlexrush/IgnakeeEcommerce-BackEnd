using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Addresses.Commands.CreateAddress
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, ShippingAddressVm>
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAddressCommandHandler(IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ShippingAddressVm> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            //var addressRecord = await _unitOfWork.Repository<Address>().GetEntityAsync(x => x.UserName == _authService.GetSessionUser(), null, false);
            var addressTarget = await _unitOfWork.Repository<Address>().GetAsync(x => x.UserName!.Equals(_authService.GetSessionUser()));
            var addressList = addressTarget.Where(x => x.UserAddress == request.Address && x.City == request.City && x.Region == request.Region && x.Country == request.Country && x.PostalCode == request.PostalCode).ToList();
            Address newAddress = new Address();
            if (!addressList.Any())
            {
                newAddress.UserName = _authService.GetSessionUser();
                newAddress.UserAddress = request.Address;
                newAddress.City = request.City;
                newAddress.Region = request.Region;
                newAddress.PostalCode = request.PostalCode;
                newAddress.Country = request.Country;
                newAddress.CreatedBy = _authService.GetSessionUser();
                newAddress.CreatedDate = DateTime.UtcNow;

                _unitOfWork.Repository<Address>().AddEntity(newAddress);
                await _unitOfWork.Complete();
                var response = _mapper.Map<ShippingAddressVm>(newAddress);
                return response;
            }
            else
            {
                throw new Exception("there is a similar address, please try with other address");
            }


        }
    }
}
