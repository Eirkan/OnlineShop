using MediatR;

namespace Customer.Core.Domain.Messaging.Events
{
    /// <summary>
    /// Represents the marker interface for an integration event.
    /// </summary>
    public interface IIntegrationEvent : INotification
    {
    }
}
