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
using Product.Domain.Common.Errors;
using ProductEntity = Product.Domain.Entities.Products.Product;


namespace Product.Application.Product.Queries.GetById;

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
            result => Ok(result),
            errors => Problem(errors)
        );
    }
}


public class GetByIdQueryHandler : IQueryHandler<GetByIdQuery, ErrorOr<GetByIdResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<GetByIdResponse>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (request.Id == Guid.Empty)
        {
            return Errors.Product.IdNullOrEmpty;
        }

        if (_productRepository.GetProductId(request.Id) is not ProductEntity product)
        {
            return Errors.Product.NullOrEmpty;
        }

        var response = _mapper.Map<GetByIdResponse>(product);
        return response;
    }
}
