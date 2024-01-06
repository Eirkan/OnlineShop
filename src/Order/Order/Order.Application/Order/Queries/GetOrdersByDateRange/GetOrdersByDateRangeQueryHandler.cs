using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Common.Abstractions;
using Order.Contracts.ApiRoutes;
using Order.Contracts.Order.GetOrdersByDateRange;
using Order.Core.Domain.Messaging;
using Order.Domain.Common.Errors;
using Order.Domain.Repositories;
using Order.Domain.ValueObjects;

namespace Order.Application.Order.Queries.Login;

[AllowAnonymous]
public class OrderController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public OrderController(
        ISender mediator
        , IMapper mapper
        )
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost(ApiRoutes.Order.GetOrdersByDateRange)]
    [ProducesResponseType(typeof(GetOrdersByDateRangeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrdersByDateRange(GetOrdersByDateRangeRequest request)
    {
        var query = _mapper.Map<GetOrdersByDateRangeQuery>(request);

        var response = await _mediator.Send(query);

        return response.Match(
            result => Ok(_mapper.Map<GetOrdersByDateRangeResponse>(result)),
            errors => Problem(errors)
        );
    }
}


public class GetOrdersByDateRangeHandler : IQueryHandler<GetOrdersByDateRangeQuery, ErrorOr<GetOrdersByDateRangeResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersByDateRangeHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<GetOrdersByDateRangeResponse>> Handle(GetOrdersByDateRangeQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        //var response = _orderRepository.GetOrdersByDate(request.startDate, request.endDate);
        //var result = _mapper.Map<GetOrdersByDateRangeResponse>(response);

        //return result;
        return new GetOrdersByDateRangeResponse(1, DateTime.Now);
    }
}
