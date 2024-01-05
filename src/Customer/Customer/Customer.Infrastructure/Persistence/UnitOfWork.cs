using Customer.Core.Common.Dependency;
using Customer.Core.Domain.Messaging.Events;
using Customer.Core.Domain.Primitives;
using Customer.Core.Domain.UnitofWork;
using Customer.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Customer.Infrastructure.Persistence
{
    internal sealed class UnitOfWork : IUnitOfWork, IScopedDependency
    {
        private readonly CustomerDbContext _dbContext;
        private readonly IDateTimeProvider _dateTime;
        private readonly IMediator _mediator;


        public UnitOfWork(
            CustomerDbContext dbContext
            , IDateTimeProvider dateTime
            , IMediator mediator)
        {
            _dbContext = dbContext;
            _dateTime = dateTime;
            _mediator = mediator;
        }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DateTime utcNow = _dateTime.UtcNow;

            UpdateAuditableEntities(utcNow);
            UpdateSoftDeletableEntities(utcNow);
            await PublishDomainEvents(cancellationToken);

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }


        /// <summary>
        /// Sets the property value of the given entity to the specified value.
        /// </summary>
        /// <param name="entityEntry">The entity entry.</param>
        /// <param name="property">The property name.</param>
        /// <param name="value">The value.</param>
        private static void SetPropertyValue(EntityEntry entityEntry, string property, object value)
            => entityEntry.Property(property).CurrentValue = value;


        /// <summary>
        /// Updates the specified entity entry's referenced entries in the deleted state to the modified state.
        /// This method is recursive.
        /// </summary>
        /// <param name="entityEntry">The entity entry.</param>
        private static void UpdateDeletedEntityEntryReferencesToModified(EntityEntry entityEntry)
        {
            if (!entityEntry.References.Any())
            {
                return;
            }

            foreach (ReferenceEntry referenceEntry in entityEntry.References.Where(r => r.TargetEntry?.State == EntityState.Deleted))
            {
                referenceEntry.TargetEntry!.State = EntityState.Modified;

                UpdateDeletedEntityEntryReferencesToModified(referenceEntry.TargetEntry);
            }
        }


        private void UpdateAuditableEntities(DateTime utcNow)
        {
            foreach (EntityEntry<IAuditableEntity> entityEntry in _dbContext.ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        SetPropertyValue(entityEntry, nameof(IAuditableEntity.CreatedOnUtc), utcNow);
                        break;
                    case EntityState.Modified:
                        SetPropertyValue(entityEntry, nameof(IAuditableEntity.ModifiedOnUtc), utcNow);
                        break;
                }
            }
        }

        /// <summary>
        /// Updates all the entities that implement the <see cref="ISoftDeletableEntity"/> interface.
        /// </summary>
        private void UpdateSoftDeletableEntities(DateTime utcNow)
        {
            foreach (EntityEntry<ISoftDeletableEntity> entityEntry in _dbContext.ChangeTracker.Entries<ISoftDeletableEntity>().Where(e => e.State == EntityState.Deleted))
            {
                entityEntry.State = EntityState.Modified;

                UpdateDeletedEntityEntryReferencesToModified(entityEntry);

                SetPropertyValue(entityEntry, nameof(ISoftDeletableEntity.IsDeleted), true);

                SetPropertyValue(entityEntry, nameof(ISoftDeletableEntity.DeletedOnUtc), utcNow);
            }
        }

        /// <summary>
        /// Publishes and then clears all the domain events that exist within the current transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task PublishDomainEvents(CancellationToken cancellationToken)
        {
            List<EntityEntry<IEntity>> aggregateRoots = _dbContext.ChangeTracker
                .Entries<IEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            List<IDomainEvent> domainEvents = aggregateRoots
                .SelectMany(entityEntry => entityEntry.Entity.DomainEvents)
                .ToList();

            aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

            IEnumerable<Task> tasks = domainEvents
                .Select(async domainEvent => await _mediator.Publish(domainEvent, cancellationToken));

            await Task.WhenAll(tasks);
        }
    }
}
