using Customer.Contracts.Customer.IntegrationEvents;
using Customer.Core.Domain.Messaging.Events;

namespace Customer.Application.Customer.Events.IntegrationEvents.EventHandling
{
    /// <summary>
    /// Represents the handler for the <see cref="CustomerCreatedIntegrationEventHandler"/> event.
    /// </summary>
    public sealed class CustomerCreatedIntegrationEventHandler : IIntegrationEventHandler<CustomerCreatedIntegrationEvent>
    {
        public CustomerCreatedIntegrationEventHandler()
        {
        }

        public async Task Handle(CustomerCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var notificationId = notification.CustomerId;
        }
    }
}
