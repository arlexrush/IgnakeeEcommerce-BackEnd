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

namespace Ecommerce.Application.Features.Taxes.Commands.UpdateTax
{
    public class UpdateTaxCommandHandler : IRequestHandler<UpdateTaxCommand, TaxVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public UpdateTaxCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TaxVm> Handle(UpdateTaxCommand request, CancellationToken cancellationToken)
        {
            var taxCurrent = await _unitOfWork!.Repository<Tax>().GetByIdAsync(request.TaxId);
            if (taxCurrent is null)
            {
                throw new Exception("Not found Tax");
            }
            taxCurrent.Id = request.TaxId;
            taxCurrent.Name = request.Name;
            taxCurrent.Percentage = request.Percentage;
            taxCurrent.ApplicationTax = request.ApplicationTax;
            var taxUpdate = await _unitOfWork.Repository<Tax>().UpdateAsync(taxCurrent);
            var response = _mapper!.Map<TaxVm>(taxUpdate);
            return response;
        }
    }
}
