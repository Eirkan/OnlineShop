using Customer.Application.Common.Abstractions;
using Customer.Contracts.Customer.IntegrationEvents;
using Customer.Core.Domain.Messaging.Events;
using Customer.Domain.Entities.Customers.Events;

namespace Customer.Application.Customer.Events.DomainEvents
{
    /// <summary>
    /// Represents the handler for the <see cref="CustomerCreatedDomainEvent"/> event.
    /// </summary>
    public sealed class UserCreatedDomainEventHandler : IDomainEventHandler<CustomerCreatedDomainEvent>
    {
        private readonly ICustomerIntegrationEventService _eventService;

        public UserCreatedDomainEventHandler(
            ICustomerIntegrationEventService eventService
            )
        {
            _eventService = eventService;
        }

        public async Task Handle(CustomerCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var integrationEvent = new UserCreatedIntegrationEvent(notification.UserId);
            await _eventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
