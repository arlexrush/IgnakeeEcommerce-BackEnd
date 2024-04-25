using AutoMapper;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, ShippingAddressVm>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAddressCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ShippingAddressVm> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var addressToDelete = await _unitOfWork.Repository<Address>().GetEntityAsync(x => x.Id == request.Id);
            if (addressToDelete == null)
            {
                throw new Exception("Item don´t found");
            }
            await _unitOfWork.Repository<Address>().DeleteAsync(addressToDelete!);

            var response = _mapper.Map<ShippingAddressVm>(addressToDelete);

            return response;
        }
    }
}
