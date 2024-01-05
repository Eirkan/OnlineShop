using Order.Core.Domain.Messaging.Events;

namespace Order.Contracts.Authentication.IntegrationEvents
{
    public record UserCreatedIntegrationEvent : IntegrationEvent
    {

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        public UserCreatedIntegrationEvent(Guid userId) => UserId = userId;

    }
}