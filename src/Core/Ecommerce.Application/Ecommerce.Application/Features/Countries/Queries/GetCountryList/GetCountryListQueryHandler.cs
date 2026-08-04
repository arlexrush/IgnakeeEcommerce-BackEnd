using AutoMapper;
using Ecommerce.Application.Features.Countries.Queries.Vm;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Countries.Queries.GetCountryList
{
    public class GetCountryListQueryHandler : IRequestHandler<GetCountryListQuery, IReadOnlyList<CountryVm>>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public GetCountryListQueryHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CountryVm>> Handle(GetCountryListQuery request, CancellationToken cancellationToken)
        {
            var countries = await _unitOfWork!.Repository<Country>().GetAsync(null, x => x.OrderBy(y => y.Name), string.Empty, false);

            var response = _mapper!.Map<IReadOnlyList<CountryVm>>(countries);
            return response;
        }
    }
}
