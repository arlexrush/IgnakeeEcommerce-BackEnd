using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specification.Users
{
    public class UserSpecification : BaseSpecification<User>
    {

        public UserSpecification(UserSpecificationParams userParams) : base(x => (string.IsNullOrEmpty(userParams.Search) ||
                                                                            x.Name!.Contains(userParams.Search) ||
                                                                            x.LastName!.Contains(userParams.Search) ||
                                                                            x.Email!.Contains(userParams.Search)))
        {
            ApplyPaging(userParams.PageSize * (userParams.PageIndex - 1), userParams.PageSize);

            if (!string.IsNullOrEmpty(userParams.Sort))
            {
                switch (userParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(x => x.Name!);
                        break;

                    case "nameDesc":
                        AddOrderByDescending(x => x.Name!);
                        break;

                    case "lastNameAsc":
                        AddOrderBy(x => x.LastName!);
                        break;

                    case "lastNameDesc":
                        AddOrderByDescending(x => x.LastName!);
                        break;

                    default:
                        AddOrderBy(x => x.LastName!);
                        break;

                }
            }
            else
            {
                AddOrderByDescending(x => x.Name!);
            }

        }

    }
}
