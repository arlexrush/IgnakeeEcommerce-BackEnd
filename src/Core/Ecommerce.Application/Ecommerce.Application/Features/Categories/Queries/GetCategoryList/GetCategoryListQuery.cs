using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Categories.Queries.GetCategoryList
{
    public class GetCategoryListQuery : IRequest<IReadOnlyList<CategoryVm>>
    {

    }
}
