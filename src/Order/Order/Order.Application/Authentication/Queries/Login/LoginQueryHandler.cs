using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Common.Abstractions;
using Order.Application.Common.Abstractions.Authentication;
using Order.Contracts.ApiRoutes;
using Order.Contracts.Authentication.Login;
using Order.Core.Domain.Messaging;
using Order.Domain.Common.Errors;
using Order.Domain.Entities.Users;
using Order.Domain.Repositories;
using Order.Domain.ValueObjects;

namespace Order.Application.Authentication.Queries.Login
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
