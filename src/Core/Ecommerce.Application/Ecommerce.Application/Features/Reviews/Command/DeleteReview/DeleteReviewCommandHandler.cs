using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Reviews.Command.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Unit>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public DeleteReviewCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var reviewtoDelete = await _unitOfWork!.Repository<Review>().GetByIdAsync(request.ReviewId);
            if (reviewtoDelete is null)
            {
                throw new BadRequestException("This Review no Exist");
            }
            _unitOfWork.Repository<Review>().DeleteEntity(reviewtoDelete);
            await _unitOfWork.Complete();
            return Unit.Value;
        }
    }



}
