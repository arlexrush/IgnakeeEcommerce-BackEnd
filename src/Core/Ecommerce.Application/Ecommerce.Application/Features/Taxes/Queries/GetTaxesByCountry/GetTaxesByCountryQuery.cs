using Ecommerce.Application.Features.Taxes.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Taxes.Queries.GetTaxesByCountry
{
    public class GetTaxesByCountryQuery : IRequest<IReadOnlyList<TaxVm>>
    {
        public int CountryId { get; set; }
    }
}
