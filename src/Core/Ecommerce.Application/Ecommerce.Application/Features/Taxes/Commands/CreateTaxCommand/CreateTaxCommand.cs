using Ecommerce.Application.Features.Taxes.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Taxes.Commands.CreateTaxCommand
{
    public class CreateTaxCommand:IRequest<TaxVm>
    {
        public string? Name { get; set; }
        public decimal? Percentage { get; set; }
        public int CountryId { get; set; }
        public int ProductId { get; set; }
    }
}
