using Ecommerce.Application.Features.Taxes.Vms;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Taxes.Commands.UpdateTax
{
    public class UpdateTaxCommand : IRequest<TaxVm>
    {
        public int TaxId { get; set; }
        public string? Name { get; set; }
        public decimal? Percentage { get; set; }
        public ApplicationTax ApplicationTax { get; set; }
    }
}
