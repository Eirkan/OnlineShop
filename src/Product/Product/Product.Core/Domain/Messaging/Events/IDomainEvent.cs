﻿using MediatR;

namespace Product.Core.Domain.Messaging.Events
{
    /// <summary>
    /// Represents a marker interface for a domain event.
    /// </summary>
    public interface IDomainEvent : INotification
    {
        /// <summary>
        /// Gets the event identifier.
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// Gets the UTC date and time the event occurred on.
        /// </summary>
        DateTime OccurredOnUtc { get; }
    }
}
