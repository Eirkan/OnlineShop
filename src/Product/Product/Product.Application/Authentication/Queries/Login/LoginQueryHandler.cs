using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Common.Abstractions;
using Product.Application.Common.Abstractions.Authentication;
using Product.Contracts.ApiRoutes;
using Product.Contracts.Authentication.Login;
using Product.Core.Domain.Messaging;
using Product.Domain.Common.Errors;
using Product.Domain.Entities.Users;
using Product.Domain.Repositories;
using Product.Domain.ValueObjects;

namespace Product.Application.Authentication.Queries.Login
{
    //[Route("auth")]
    [AllowAnonymous]
    public class AuthenticationController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(
            ISender mediator
            , IMapper mapper
            )
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost(ApiRoutes.Authentication.Login)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);
            //var query = new LoginQuery(request.Email, request.Password);

            var authResult = await _mediator.Send(query);

            if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
            {
                return Problem(statusCode: StatusCodes.Status401Unauthorized, title: authResult.FirstError.Description);
            }

            return authResult.Match(
                result => Ok(_mapper.Map<LoginResponse>(result)),
                errors => Problem(errors)
            );
        }
    }


    public class LoginQueryHandler : IQueryHandler<LoginQuery, ErrorOr<LoginResponse>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<LoginResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var email = Email.Create(request.Email);
            if (email.IsError)
            {
                return email.Errors;
            }

            if (_userRepository.GetUserByEmail(email.Value) is not User user)
            {
                return Errors.Authentication.InvalidCredentials;
            }

            if (user.Password != request.Password)
            {
                return Errors.Authentication.InvalidCredentials;
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new LoginResponse(user.FirstName, user.LastName, user.Email, user.Password, token);
            //return new AuthenticationResult(user, token);
        }
    }
}
