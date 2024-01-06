using Order.Application.Common.Abstractions;
using Order.Contracts.Orders.IntegrationEvents;
using Order.Core.Domain.Messaging.Events;
using Order.Domain.Entities.OrderAggregate.Event;

namespace Order.Application.Order.Events.DomainEvents
{
    /// <summary>
    /// Represents the handler for the <see cref="UserCreatedDomainEvent"/> event.
    /// </summary>
    public sealed class OrderStartedDomainEventHandler : IDomainEventHandler<OrderStartedDomainEvent>
    {
        private readonly IOrderIntegrationEventService _eventService;

        public OrderStartedDomainEventHandler(
            IOrderIntegrationEventService eventService
            )
        {
            _eventService = eventService;
        }

        public async Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var integrationEvent = new OrderStartedIntegrationEvent(notification.Order.Id, notification.Order.OrderStatus.Name, notification.UserName, notification.UserId);
            await _eventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
