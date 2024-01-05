using Product.Core.Common.Dependency;
using Product.Domain.Entities.Users;
using Product.Domain.ValueObjects;

namespace Product.Domain.Repositories
{
    public interface IUserRepository : IScopedDependency
    {
        User? GetUserByEmail(Email email);

        void Insert(User user);

        void Update(User user);
    }
}
