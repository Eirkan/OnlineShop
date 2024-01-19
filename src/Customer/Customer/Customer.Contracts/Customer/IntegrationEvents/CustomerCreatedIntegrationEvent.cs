using Customer.Core.Domain.Messaging.Events;

namespace Customer.Contracts.Customer.IntegrationEvents
{
    public record CustomerCreatedIntegrationEvent : IntegrationEvent
    {

        /// <summary>
        /// Gets the Customer identifier.
        /// </summary>
        public Guid CustomerId { get; }

        public CustomerCreatedIntegrationEvent(Guid CustomerId) => this.CustomerId = CustomerId;

    }
}