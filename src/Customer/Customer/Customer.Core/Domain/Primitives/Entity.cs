using Customer.Core.Domain.Messaging.Events;

namespace Customer.Core.Domain.Primitives
{
    public interface IEntity
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }


    public abstract class Entity<TId> : IEquatable<Entity<TId>>, IEntity
        where TId : notnull
    {
        public TId Id { get; protected set; }

        protected Entity(TId id)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            return obj is Entity<TId> entity && Id.Equals(entity.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TId> left, Entity<TId> right)
            => Equals(left, right);

        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        => !Equals(left, right);

        public bool Equals(Entity<TId>? other)
        {
            return Equals((object?)other);
        }


        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        /// <summary>
        /// Gets the domain events. This collection is readonly.
        /// </summary>
        public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Clears all the domain events from the <see cref="IEntity"/>.
        /// </summary>
        public virtual void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        /// <summary>
        /// Adds the specified <see cref="IDomainEvent"/> to the <see cref="IEntity"/>.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        protected virtual void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}