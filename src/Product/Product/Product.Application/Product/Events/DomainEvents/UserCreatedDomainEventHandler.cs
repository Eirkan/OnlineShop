using Product.Application.Common.Abstractions;
using Product.Contracts.Product.IntegrationEvents;
using Product.Core.Domain.Messaging.Events;
using Product.Domain.Entities.Products.Events;

namespace Product.Application.Product.Events.DomainEvents
{
    /// <summary>
    /// Represents the handler for the <see cref="ProductCreatedDomainEvent"/> event.
    /// </summary>
    public sealed class UserCreatedDomainEventHandler : IDomainEventHandler<ProductCreatedDomainEvent>
    {
        private readonly IProductIntegrationEventService _eventService;

        public UserCreatedDomainEventHandler(
            IProductIntegrationEventService eventService
            )
        {
            _eventService = eventService;
        }

        public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var integrationEvent = new ProductCreatedIntegrationEvent(notification.Id);
            await _eventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
