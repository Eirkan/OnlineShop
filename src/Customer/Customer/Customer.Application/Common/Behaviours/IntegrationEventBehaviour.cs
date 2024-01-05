﻿using Customer.Application.Common.Abstractions;
using MediatR;

namespace Customer.Application.Common.Behaviours
{
    public sealed class IntegrationEventBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ICustomerIntegrationEventService _eventService;

        public IntegrationEventBehaviour(ICustomerIntegrationEventService eventService)
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