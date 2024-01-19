using Customer.Application.Common.Abstractions;
using Customer.Contracts.ApiRoutes;
using Customer.Contracts.Customer.Insert;
using Customer.Core.Domain.Messaging;
using Customer.Domain.Common.Errors;
using Customer.Domain.Repositories;
using Customer.Domain.ValueObjects;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerEntity = Customer.Domain.Entities.Customers.Customer;

namespace Customer.Application.Customer.Commands.Insert
{
    [AllowAnonymous]
    public class CustomerController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public CustomerController(
            ISender mediator
            , IMapper mapper
            )
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost(ApiRoutes.Customer.Insert)]
        [ProducesResponseType(typeof(InsertResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HandleAsyc(InsertRequest request)
        {
            var command = _mapper.Map<InsertCommand>(request);
            var response = await _mediator.Send(command);

            return response.Match(
                result => Ok(_mapper.Map<InsertResponse>(result)),
                errors => Problem(errors)
            );
        }
    }



    public class InsertCommandHandler : ICommandHandler<InsertCommand, ErrorOr<InsertResponse>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICustomerIntegrationEventService _eventService;

        public InsertCommandHandler(
            ICustomerRepository customerRepository,
            IMediator mediator,
            IMapper mapper,
            ICustomerIntegrationEventService eventService
            )
        {
            _customerRepository = customerRepository;
            _mediator = mediator;
            _mapper = mapper;
            _eventService = eventService;
        }

        public async Task<ErrorOr<InsertResponse>> Handle(InsertCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var email = Email.Create(request.Email);
            if (email.IsError)
            {
                return email.Errors;
            }

            if (_customerRepository.GetCustomerByEmail(email.Value) is not null)
            {
                return Errors.Customer.DuplicateEmail;
            }

            var customer = _mapper.Map<CustomerEntity>(request);
            _customerRepository.Insert(customer);

            customer.InsertCustomer();


            return new InsertResponse(Guid.NewGuid(), request.FirstName, request.LastName, request.Email);
        }
    }
}
