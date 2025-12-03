using Application.DTO;
using Application.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetReviewsByOrderIdQueryHandler : IRequestHandler<GetReviewsByOrderIdQuery, IEnumerable<ReviewDto>>
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;

        public GetReviewsByOrderIdQueryHandler(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDto>> Handle(GetReviewsByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _repository.GetByOrderIdAsync(request.OrderId);
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }
    }
}
