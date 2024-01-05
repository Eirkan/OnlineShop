using Product.Core.Domain.Messaging.Events;

namespace Product.Contracts.Authentication.IntegrationEvents
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