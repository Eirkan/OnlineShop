using Customer.Core.Domain.Primitives;
using Customer.Core.Specification;
using System.Linq.Expressions;

namespace Customer.Infrastructure.Persistence.Specifications
{
    public class SpecificationById<T> : Specification<T> where T : Entity<long>, new()
    {
        private readonly long _id;

        public SpecificationById(long id)
        {
            _id = id;
        }

        public static SpecificationById<T> Create(long id)
        {
            return new SpecificationById<T>(id);
        }
        public override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.Id == _id;
        }
    }

    public class SpecificationByGuidId<T> : Specification<T> where T : Entity<Guid>, new()
    {
        private readonly Guid _id;

        public SpecificationByGuidId(Guid id)
        {
            _id = id;
        }

        public static SpecificationByGuidId<T> Create(Guid id)
        {
            return new SpecificationByGuidId<T>(id);
        }
        public override Expression<Func<T, bool>> ToExpression()
        {
            return x => x.Id == _id;
        }
    }
}
