using Customer.Application.Common.Abstractions;
using Customer.Contracts.Authentication.IntegrationEvents;
using Customer.Core.Domain.Messaging.Events;
using Customer.Domain.Entities.Users.Events;

namespace Customer.Application.Authentication.Events.DomainEvents
{
    /// <summary>
    /// Represents the handler for the <see cref="UserCreatedDomainEvent"/> event.
    /// </summary>
    public sealed class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
    {
        private readonly ICustomerIntegrationEventService _eventService;

        public UserCreatedDomainEventHandler(
            ICustomerIntegrationEventService eventService
            )
        {
            _eventService = eventService;
        }

        public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var integrationEvent = new UserCreatedIntegrationEvent(notification.UserId);
            await _eventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
