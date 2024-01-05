using Product.Core.Domain.Messaging.Events;

namespace Product.Core.Domain.Primitives
{
    public abstract class AggregateRoot<TId> : Entity<TId>
        where TId : notnull
    {
        protected AggregateRoot(TId id) : base(id)
        {
        }
    }
}
