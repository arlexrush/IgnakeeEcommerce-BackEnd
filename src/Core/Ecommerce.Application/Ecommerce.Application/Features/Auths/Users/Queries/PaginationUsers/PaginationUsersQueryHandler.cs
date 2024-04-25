using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specification.Users;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Auths.Users.Queries.PaginationUsers
{
    public class PaginationUsersQueryHandler : IRequestHandler<PaginationUsersQuery, PaginationVm<User>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaginationUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationVm<User>> Handle(PaginationUsersQuery request, CancellationToken cancellationToken)
        {
            var userSpecificationParams = new UserSpecificationParams
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort,
            };

            var spec = new UserSpecification(userSpecificationParams);

            var users=await _unitOfWork.Repository<User>().GetAllByIdWithSpec(spec);

            var totalUsers = await _unitOfWork.Repository<User>().CountAsync(spec);

            var rounded = Math.Ceiling((Convert.ToDecimal(totalUsers))/(Convert.ToDecimal(request.PageSize)));

            var totalPage = Convert.ToInt32(rounded);

            var userByPages = users.Count();

            var pagination = new PaginationVm<User>
            {
                Count = totalUsers,
                Data = users,
                PageCount = totalPage,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                ResultByPage = userByPages,
            };
            return pagination;
        }
    }
}
