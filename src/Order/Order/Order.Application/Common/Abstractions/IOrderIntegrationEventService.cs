using Order.Core.Domain.Messaging.Events;

namespace Order.Application.Common.Abstractions
{
    public interface IOrderIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync();
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}