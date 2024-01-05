using MediatR;

namespace Product.Core.Domain.Messaging.Events
{
    /// <summary>
    /// Represents the marker interface for an integration event.
    /// </summary>
    public interface IIntegrationEvent : INotification
    {
    }
}
