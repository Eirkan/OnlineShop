using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Common.Abstractions;
using Order.Application.Common.Abstractions.Authentication;
using Order.Application.Services.Authentication.Common;
using Order.Contracts.ApiRoutes;
using Order.Contracts.Authentication.IntegrationEvents;
using Order.Contracts.Authentication.Register;
using Order.Core.Domain.Messaging;
using Order.Domain.Common.Errors;
using Order.Domain.Entities.Users;
using Order.Domain.Repositories;
using Order.Domain.ValueObjects;

namespace Order.Application.Authentication.Commands.Register
{
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

        [HttpPost(ApiRoutes.Authentication.Register)]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterCommand>(request);
            var registerResult = await _mediator.Send(command);

            return registerResult.Match(
                result => Ok(_mapper.Map<RegisterResponse>(result)),
                errors => Problem(errors)
            );
        }
    }



    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOrderIntegrationEventService _eventService;

        public RegisterCommandHandler(
            IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepository,
            IMediator mediator,
            IMapper mapper,
            IOrderIntegrationEventService eventService
            )
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _mediator = mediator;
            _mapper = mapper;
            _eventService = eventService;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var email = Email.Create(request.Email);
            if (email.IsError)
            {
                return email.Errors;
            }

            if (_userRepository.GetUserByEmail(email.Value) is not null)
            {
                return Errors.User.DuplicateEmail;
            }

            var user = _mapper.Map<User>(request);
            _userRepository.Insert(user);

            user.RegisterUser();
            //await _mediator.Publish(new UserCreatedDomainEvent(user.Id, user.FirstName, user.LastName, user.Email), cancellationToken);


            var token = _jwtTokenGenerator.GenerateToken(user);

            //var integrationEvent = new UserCreatedIntegrationEvent(user.GetUserCreatedDomainEvent().UserId);
            //await _eventService.AddAndSaveEventAsync(integrationEvent);

            return new AuthenticationResult(user, token);
        }
    }
}
