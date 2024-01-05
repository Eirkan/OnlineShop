using Product.Application.Common.Abstractions;
using Product.Contracts.Authentication.IntegrationEvents;
using Product.Core.Domain.Messaging.Events;
using Product.Domain.Entities.Users.Events;

namespace Product.Application.Authentication.Events.DomainEvents
{
    /// <summary>
    /// Represents the handler for the <see cref="UserCreatedDomainEvent"/> event.
    /// </summary>
    public sealed class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
    {
        private readonly IProductIntegrationEventService _eventService;

        public UserCreatedDomainEventHandler(
            IProductIntegrationEventService eventService
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
