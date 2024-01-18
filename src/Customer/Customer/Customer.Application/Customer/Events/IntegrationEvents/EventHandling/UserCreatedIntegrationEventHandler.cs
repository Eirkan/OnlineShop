﻿using Customer.Contracts.Customer.IntegrationEvents;
using Customer.Core.Domain.Messaging.Events;

namespace Customer.Application.Customer.Events.IntegrationEvents.EventHandling
{
    /// <summary>
    /// Represents the handler for the <see cref="UserCreatedIntegrationEventHandler"/> event.
    /// </summary>
    public sealed class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<CustomerCreatedIntegrationEvent>
    {
        public UserCreatedIntegrationEventHandler()
        {
        }

        public async Task Handle(CustomerCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var notificationId = notification.CustomerId;
        }
    }
}