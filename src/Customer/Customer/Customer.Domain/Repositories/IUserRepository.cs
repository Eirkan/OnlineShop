using Customer.Core.Common.Dependency;
using Customer.Domain.Entities.Users;
using Customer.Domain.ValueObjects;

namespace Customer.Domain.Repositories
{
    public interface IUserRepository : IScopedDependency
    {
        User? GetUserByEmail(Email email);

        void Insert(User user);

        void Update(User user);
    }
}
