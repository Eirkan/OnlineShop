using Product.Contracts.Authentication.IntegrationEvents;
using Product.Core.Domain.Messaging.Events;

namespace Product.Application.Authentication.Events.IntegrationEvents.EventHandling
{
    /// <summary>
    /// Represents the handler for the <see cref="UserCreatedIntegrationEventHandler"/> event.
    /// </summary>
    public sealed class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        public UserCreatedIntegrationEventHandler()
        {
        }

        public async Task Handle(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var notificationId = notification.UserId;
        }
    }
}
