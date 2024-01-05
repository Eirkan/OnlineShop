using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product.Application.Common.Abstractions;
using Product.Core.Domain.Messaging.Events;
using Product.Core.IntegrationEventLogEF.Services;
using System.Data.Common;

namespace Product.Infrastructure.Persistence.IntegrationEvents
{
    public class ProductIntegrationEventService : IProductIntegrationEventService//, ITransientDependency
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IMediator _eventBus;
        private readonly ProductDbContext _dbContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<ProductIntegrationEventService> _logger;

        public static string Namespace = typeof(ProductIntegrationEventService).Namespace!;
        public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public ProductIntegrationEventService(IMediator eventBus,
            ProductDbContext dbContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
            ILogger<ProductIntegrationEventService> logger)
        {
            _eventBus = eventBus;
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventLogService = _integrationEventLogServiceFactory(_dbContext.Database.GetDbConnection());
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishEventsThroughEventBusAsync()
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync();

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.EventId, AppName, logEvt.IntegrationEvent);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    await _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", logEvt.EventId, AppName);

                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            await _eventLogService.SaveEventAsync(evt);
        }
    }

}
