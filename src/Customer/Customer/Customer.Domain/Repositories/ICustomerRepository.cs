using Customer.Core.Common.Dependency;
using Customer.Domain.ValueObjects;

namespace Customer.Domain.Repositories
{
    public interface ICustomerRepository : IScopedDependency
    {
        Entities.Customers.Customer? GetCustomerByEmail(Email email);

        void Insert(Entities.Customers.Customer customer);

        void Update(Entities.Customers.Customer customer);
    }
}
