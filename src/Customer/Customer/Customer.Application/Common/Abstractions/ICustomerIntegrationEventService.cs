using Customer.Core.Domain.Messaging.Events;

namespace Customer.Application.Common.Abstractions
{
    public interface ICustomerIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync();
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}