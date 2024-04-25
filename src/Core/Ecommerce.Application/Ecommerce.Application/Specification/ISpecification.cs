using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification
{
    public interface ISpecification<T>
    {
        // ISpecification define la estructura común de una especificación de consulta parametrizable: propiedades para filtros, ordenamiento, paginación, etc.
        Expression<Func<T, bool>>? Criteria { get; }

        List<Expression<Func<T, object>>> Includes { get; }

        Expression<Func<T, object>>? OrderBy { get; }

        Expression<Func<T, object>>? OrderByDescending { get; }

        int? Take { get; }

        int? Skip { get; }

        bool IsPagingEnable { get; }

    }
}
