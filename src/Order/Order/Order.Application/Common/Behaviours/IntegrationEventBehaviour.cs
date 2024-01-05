using MediatR;
using Order.Application.Common.Abstractions;

namespace Order.Application.Common.Behaviours
{
    public sealed class IntegrationEventBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IOrderIntegrationEventService _eventService;

        public IntegrationEventBehaviour(IOrderIntegrationEventService eventService)
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