using AutoMapper;
using Ecommerce.Application.Features.Taxes.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Taxes.Queries.GetTaxesByCountry
{
    public class GetTaxesByCountryQueryHandler : IRequestHandler<GetTaxesByCountryQuery, IReadOnlyList<TaxVm>>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public GetTaxesByCountryQueryHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<TaxVm>> Handle(GetTaxesByCountryQuery request, CancellationToken cancellationToken)
        {
            var includes= new List<Expression<Func<Tax, object>>>();
            includes.Add(x => x.TaxByProducts!.OrderBy(y=>y.ProductId));
            var taxes = await _unitOfWork!.Repository<Tax>().GetAsync(x=>x.CountryId==request.CountryId, null, includes, false);
            foreach(Tax item in taxes)
            {
                var country = await _unitOfWork.Repository<Country>().GetEntityAsync(x=>x.Id==item.CountryId);
                item.Country = country;
            }
            var response = _mapper!.Map<IReadOnlyList<TaxVm>>(taxes);
            return response;
        }
    }
}
