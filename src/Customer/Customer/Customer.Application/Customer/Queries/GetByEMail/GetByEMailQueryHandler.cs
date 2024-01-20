using Customer.Application.Common.Abstractions;
using Customer.Contracts.ApiRoutes;
using Customer.Contracts.Customer.GetByEMail;
using Customer.Core.Domain.Messaging;
using Customer.Domain.Common.Errors;
using Customer.Domain.Repositories;
using Customer.Domain.ValueObjects;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static StackExchange.Redis.Role;
using CustomerEntity = Customer.Domain.Entities.Customers.Customer;

namespace Customer.Application.Customer.Queries.GetByEMail
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

        [HttpPost(ApiRoutes.Customer.GetByEMail)]
        [ProducesResponseType(typeof(GetByEMailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HandleAsync(GetByEMailRequest request)
        {
            var query = _mapper.Map<GetByEMailQuery>(request);
            var response = await _mediator.Send(query);

            return response.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }


    public class GetByEMailQueryHandler : IQueryHandler<GetByEMailQuery, ErrorOr<GetByEMailResponse>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetByEMailQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<GetByEMailResponse>> Handle(GetByEMailQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var email = Email.Create(request.Email);
            if (email.IsError)
            {
                return email.Errors;
            }

            if (_customerRepository.GetCustomerByEmail(email.Value) is not CustomerEntity customer)
            {
                return Errors.Customer.NullOrEmpty;
            }

            var result = _mapper.Map<GetByEMailResponse>(customer);
            return result;
        }
    }
}
