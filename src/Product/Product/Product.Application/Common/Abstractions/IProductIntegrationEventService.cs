using Product.Core.Domain.Messaging.Events;

namespace Product.Application.Common.Abstractions
{
    public interface IProductIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync();
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}