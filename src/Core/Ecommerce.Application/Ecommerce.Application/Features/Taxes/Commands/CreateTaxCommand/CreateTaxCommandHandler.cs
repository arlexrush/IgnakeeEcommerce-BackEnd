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

namespace Ecommerce.Application.Features.Taxes.Commands.CreateTaxCommand
{
    public class CreateTaxCommandHandler : IRequestHandler<CreateTaxCommand, TaxVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public CreateTaxCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TaxVm> Handle(CreateTaxCommand request, CancellationToken cancellationToken)
        {
            var product= await _unitOfWork!.Repository<Product>().GetByIdAsync(request.ProductId);            
            if (product is null)
            {
                throw new Exception("No found Product, search a valid ProductId");
            }

            var country = await _unitOfWork!.Repository<Country>().GetByIdAsync(request.CountryId);
            if (country is null)
            {
                throw new Exception("No found Country, search a valid CountryId");
            }

            var existTaxByCountry = await _unitOfWork.Repository<Tax>().GetAsync(x => x.CountryId == country.Id && x.Percentage == request.Percentage);

            if (existTaxByCountry.Any())
            {
                throw new Exception("Can´t duplicate tax for a same country");
            }

            var taxEntity = new Tax()
            {
                Name = $"{country.Name!.ToUpper()} IVA-{request.Percentage}"?? request.Name,
                Percentage = request.Percentage,
                CountryId = request.CountryId,
                ApplicationTax=ApplicationTax.Item
            };

            
            var taxNew=await _unitOfWork!.Repository<Tax>().AddAsync(taxEntity);
            var taxByProductEntity=new TaxByProduct() { TaxId=taxNew.Id, ProductId=product.Id, IsActivated=true };
            var taxByProductNew= await _unitOfWork!.Repository<TaxByProduct>().AddAsync(taxByProductEntity);
            
            var taxesByProductEntity =new List<TaxByProduct>() { taxByProductNew };
            taxNew.TaxByProducts = taxesByProductEntity;
            var taxUpdated=await _unitOfWork.Repository<Tax>().UpdateAsync(taxNew);

            var taxResponse = _mapper!.Map<TaxVm>(taxUpdated);

            return taxResponse;
        }
    }
}
