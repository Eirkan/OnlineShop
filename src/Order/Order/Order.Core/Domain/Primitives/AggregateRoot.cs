using Order.Core.Domain.Messaging.Events;

namespace Order.Core.Domain.Primitives
{
    public abstract class AggregateRoot<TId> : Entity<TId>
        where TId : notnull
    {
        protected AggregateRoot(TId id) : base(id)
        {
        }
    }
}
