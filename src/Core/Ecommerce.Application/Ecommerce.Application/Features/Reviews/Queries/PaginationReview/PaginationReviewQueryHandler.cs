using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specification.Reviews;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Reviews.Queries.PaginationReview
{
    public class PaginationReviewQueryHandler : IRequestHandler<PaginationReviewQuery, PaginationVm<ReviewVm>>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public PaginationReviewQueryHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<ReviewVm>> Handle(PaginationReviewQuery request, CancellationToken cancellationToken)
        {
            var param = new ReviewSpecificationParams
            {
                ProductId = request.ProductId,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort

            };
            var spec = new ReviewSpecification(param);
            var reviews = await _unitOfWork!.Repository<Review>().GetAllByIdWithSpec(spec);

            // Specification for counting total registers
            var specCount = new ReviewForCountingSpecification(param);

            //Total Registers
            var count = await _unitOfWork.Repository<Review>().CountAsync(specCount);

            //Number of register for page selected by user
            var pageSize = request.PageSize;

            //Number of pages
            var pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(count) / Convert.ToDecimal(request.PageSize)));

            // Current Page
            var pageIndex = request.PageIndex;

            // Number of registers resultings by page
            var resultByPage = reviews.Count;

            // var data = _mapper!.Map<IReadOnlyList<ReviewVm>, IReadOnlyList<Review>>(reviews);
            var reviewVms = _mapper!.Map<IReadOnlyList<ReviewVm>>(reviews);
            var pagination = new PaginationVm<ReviewVm>
            {
                Data = reviewVms,
                Count = count,
                PageCount = pageCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultByPage = resultByPage
            };

            return pagination;
        }
    }
}
