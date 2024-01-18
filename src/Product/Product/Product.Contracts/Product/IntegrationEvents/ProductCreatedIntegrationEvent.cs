using Product.Core.Domain.Messaging.Events;

namespace Product.Contracts.Product.IntegrationEvents
{
    public record ProductCreatedIntegrationEvent : IntegrationEvent
    {

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid ProductId { get; }

        public ProductCreatedIntegrationEvent(Guid productId) => ProductId = productId;

    }
}