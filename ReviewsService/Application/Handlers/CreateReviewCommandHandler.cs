using Application.Commands;
using Application.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;

        public CreateReviewCommandHandler(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Review(request.CustomerId, request.OrderId, new Rating(request.Rating), request.Comment);
            await _repository.AddAsync(review);
            return _mapper.Map<ReviewDto>(review);
        }
    }
}
