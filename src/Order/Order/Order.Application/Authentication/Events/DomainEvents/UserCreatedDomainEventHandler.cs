using Order.Application.Common.Abstractions;
using Order.Contracts.Authentication.IntegrationEvents;
using Order.Core.Domain.Messaging.Events;
using Order.Domain.Entities.Users.Events;

namespace Order.Application.Authentication.Events.DomainEvents
{
    /// <summary>
    /// Represents the handler for the <see cref="UserCreatedDomainEvent"/> event.
    /// </summary>
    public sealed class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
    {
        private readonly IOrderIntegrationEventService _eventService;

        public UserCreatedDomainEventHandler(
            IOrderIntegrationEventService eventService
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
