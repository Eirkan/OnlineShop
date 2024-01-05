using Product.Core.Common.Dependency;

namespace Product.Domain.Services
{
    public interface IDateTimeProvider : ISingletonDependency
    {
        DateTime UtcNow { get; }
    }
}
