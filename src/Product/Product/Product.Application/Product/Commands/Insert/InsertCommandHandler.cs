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
using ProductEntity = Product.Domain.Entities.Products.Product;

namespace Product.Application.Product.Commands.Insert;

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
        var response = await _mediator.Send(command);

        return response.Match(
            result => Ok(_mapper.Map<InsertResponse>(result)),
            errors => Problem(errors)
        );
    }
}



public class InsertCommandHandler : ICommandHandler<InsertCommand, ErrorOr<InsertResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public InsertCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<InsertResponse>> Handle(InsertCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var product = _mapper.Map<ProductEntity>(request);
        _productRepository.Insert(product);

        product.InsertProduct();

        var response = _mapper.Map<InsertResponse>(product);
        return response;
    }
}
