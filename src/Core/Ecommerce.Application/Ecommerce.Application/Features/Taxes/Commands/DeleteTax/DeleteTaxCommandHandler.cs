using AutoMapper;
using Ecommerce.Application.Features.Taxes.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Taxes.Commands.DeleteTax
{
    public class DeleteTaxCommandHandler : IRequestHandler<DeleteTaxCommand, TaxVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public DeleteTaxCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TaxVm> Handle(DeleteTaxCommand request, CancellationToken cancellationToken)
        {
            var taxTarget = await _unitOfWork!.Repository<Tax>().GetByIdAsync(request.Id);
            await _unitOfWork!.Repository<Tax>().DeleteAsync(taxTarget);
            var response = _mapper!.Map<TaxVm>(taxTarget);
            return response;
        }
    }
}
