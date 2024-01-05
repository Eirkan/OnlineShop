using Order.Core.Common.Dependency;

namespace Order.Domain.Services
{
    public interface IDateTimeProvider : ISingletonDependency
    {
        DateTime UtcNow { get; }
    }
}
