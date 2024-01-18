using Product.Contracts.Product.IntegrationEvents;
using Product.Core.Domain.Messaging.Events;

namespace Product.Application.Product.Events.IntegrationEvents.EventHandling
{
    /// <summary>
    /// Represents the handler for the <see cref="ProductCreatedIntegrationEventHandler"/> event.
    /// </summary>
    public sealed class ProductCreatedIntegrationEventHandler : IIntegrationEventHandler<ProductCreatedIntegrationEvent>
    {
        public ProductCreatedIntegrationEventHandler()
        {
        }

        public async Task Handle(ProductCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var notificationId = notification.ProductId;
        }
    }
}
