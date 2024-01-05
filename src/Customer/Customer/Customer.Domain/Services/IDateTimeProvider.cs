using Customer.Core.Common.Dependency;

namespace Customer.Domain.Services
{
    public interface IDateTimeProvider : ISingletonDependency
    {
        DateTime UtcNow { get; }
    }
}
