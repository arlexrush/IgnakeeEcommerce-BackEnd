using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification
{
    public class BaseSpecification<T>:ISpecification<T>
    {
        //implementa ISpecification proporcionando las propiedades y métodos base para construir specifications.
        public BaseSpecification() { }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria=criteria;
        }

        public Expression<Func<T, bool>>? Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } =new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>>? OrderBy { get; private set; }

        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        public int? Take { get; private set; }

        public int? Skip { get; private set; }

        public bool IsPagingEnable { get; private set; }

        protected void AddOrderBy(Expression<Func<T, object>>? orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>>? orderByExpression)
        {
            OrderByDescending= orderByExpression;
        }

        protected void ApplyPaging(int? skip, int? take)
        {
            Take = take;
            Skip = skip;
            IsPagingEnable= true;

        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }
}
