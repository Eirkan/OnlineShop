using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Common.Abstractions;
using Product.Contracts.ApiRoutes;
using Product.Contracts.Product.Insert;
using Product.Core.Domain.Messaging;
using Product.Domain.Repositories;

namespace Product.Application.Product.Commands.Insert
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

        [HttpPost(ApiRoutes.Product.Insert)]
        [ProducesResponseType(typeof(InsertResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HandleAsync(InsertRequest request)
        {
            var command = _mapper.Map<InsertCommand>(request);
            var registerResult = await _mediator.Send(command);

            return registerResult.Match(
                result => Ok(_mapper.Map<InsertResponse>(result)),
                errors => Problem(errors)
            );
        }
    }



    public class InsertCommandHandler : ICommandHandler<InsertCommand, ErrorOr<InsertResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IProductIntegrationEventService _eventService;

        public InsertCommandHandler(
            IProductRepository productRepository,
            IMediator mediator,
            IMapper mapper,
            IProductIntegrationEventService eventService
            )
        {
            _productRepository = productRepository;
            _mediator = mediator;
            _mapper = mapper;
            _eventService = eventService;
        }

        public async Task<ErrorOr<InsertResponse>> Handle(InsertCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            //var email = Email.Create(request.Email);
            //if (email.IsError)
            //{
            //    return email.Errors;
            //}

            //if (_userRepository.GetUserByEmail(email.Value) is not null)
            //{
            //    return Errors.User.DuplicateEmail;
            //}

            var product = _mapper.Map<Domain.Entities.Products.Product>(request);
            //_productRepository.Insert(product);
            //product.InsertProduct();

            var response = new Domain.Entities.Products.Product(Guid.NewGuid(), request.Name, request.Description, request.Price, request.AvailableStock);
            var result = _mapper.Map<InsertResponse>(response);
            return result;
        }
    }
}
