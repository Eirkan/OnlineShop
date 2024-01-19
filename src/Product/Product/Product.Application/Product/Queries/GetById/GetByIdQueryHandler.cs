using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Common.Abstractions;
using Product.Contracts.ApiRoutes;
using Product.Contracts.Product.GetById;
using Product.Core.Domain.Messaging;
using Product.Domain.Repositories;

namespace Product.Application.Product.Queries.GetById
{
    [AllowAnonymous]
    public class ProductController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public ProductController(
            ISender mediator
            , IMapper mapper
            )
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost(ApiRoutes.Product.GetById)]
        [ProducesResponseType(typeof(GetByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HandleAsync(GetByIdRequest request)
        {
            var query = _mapper.Map<GetByIdQuery>(request);
            var response = await _mediator.Send(query);

            return response.Match(
                result => Ok(_mapper.Map<GetByIdResponse>(result)),
                errors => Problem(errors)
            );
        }
    }


    public class GetByIdQueryHandler : IQueryHandler<GetByIdQuery, ErrorOr<GetByIdResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ErrorOr<GetByIdResponse>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            //var email = Email.Create(request.Email);
            //if (email.IsError)
            //{
            //    return email.Errors;
            //}

            //if (_userRepository.GetUserByEmail(email.Value) is not User user)
            //{
            //    return Errors.Authentication.InvalidCredentials;
            //}

            //if (user.Password != request.Password)
            //{
            //    return Errors.Authentication.InvalidCredentials;
            //}



            return new GetByIdResponse(request.Id, "Name", "Description", 12.45m, 3);
            //return new AuthenticationResult(user, token);
        }
    }
}
