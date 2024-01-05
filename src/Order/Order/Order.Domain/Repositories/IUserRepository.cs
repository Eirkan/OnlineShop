using Order.Core.Common.Dependency;
using Order.Domain.Entities.Users;
using Order.Domain.ValueObjects;

namespace Order.Domain.Repositories
{
    public interface IUserRepository : IScopedDependency
    {
        User? GetUserByEmail(Email email);

        void Insert(User user);

        void Update(User user);
    }
}
