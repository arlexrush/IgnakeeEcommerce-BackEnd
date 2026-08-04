using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification.Users
{
    public class UserForCountingSpecification : BaseSpecification<User>
    {
        public UserForCountingSpecification(UserSpecificationParams userParams) : base(x => (string.IsNullOrEmpty(userParams.Search) ||
                                                                                     x.Name!.Contains(userParams.Search!) ||
                                                                                     x.LastName!.Contains(userParams.Search) ||
                                                                                     x.Email!.Contains(userParams.Search)))
        {
        }
    }
}
