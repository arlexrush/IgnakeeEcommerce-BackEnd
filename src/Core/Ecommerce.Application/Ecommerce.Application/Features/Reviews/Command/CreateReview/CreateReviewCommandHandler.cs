using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Reviews.Command.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;

        public CreateReviewCommandHandler(IUnitOfWork? unitOfWork, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReviewVm> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var newReview=_mapper!.Map<Review>(request);
            _unitOfWork!.Repository<Review>().AddEntity(newReview);
            var complete=await _unitOfWork.Complete();
            if (complete<=0)
            {
                throw new Exception("Con´t to create this review");
            }
            var response=_mapper!.Map<ReviewVm>(newReview);
            return response;
        }
    }
}
