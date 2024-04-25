using Ecommerce.Application.Features.Taxes.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Taxes.Commands.DeleteTax
{
    public class DeleteTaxCommand:IRequest<TaxVm>
    {
        public int Id { get; set; }
    }
}
