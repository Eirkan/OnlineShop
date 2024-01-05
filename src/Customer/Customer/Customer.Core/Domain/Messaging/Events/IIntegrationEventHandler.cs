﻿using MediatR;

namespace Customer.Core.Domain.Messaging.Events
{
    /// <summary>
    /// Represents a integration event handler interface.
    /// </summary>
    /// <typeparam name="TIntegrationEvent">The integration event type.</typeparam>
    public interface IIntegrationEventHandler<in TIntegrationEvent> :
        INotificationHandler<TIntegrationEvent>
        where TIntegrationEvent : IIntegrationEvent
    {
    }
}
