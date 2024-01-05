using MediatR;
using Product.Application.Common.Abstractions;

namespace Product.Application.Common.Behaviours
{
    public sealed class IntegrationEventBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IProductIntegrationEventService _eventService;

        public IntegrationEventBehaviour(IProductIntegrationEventService eventService)
        {
            _eventService = eventService;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            TResponse response = await next();

            await _eventService.PublishEventsThroughEventBusAsync();

            return response;
        }
    }
}