namespace Order.Core.Domain.Messaging.Events
{

    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);
    }
}
