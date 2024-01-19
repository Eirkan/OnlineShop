using Customer.Domain.Repositories;
using Customer.Domain.ValueObjects;
using Customer.Infrastructure.Persistence.Specifications;
using CustomerEntity = Customer.Domain.Entities.Customers.Customer;

namespace Customer.Infrastructure.Persistence.Repositories
{
    internal sealed class CustomerRepository : GenericRepository<CustomerEntity>, ICustomerRepository
    {
        private readonly IDbContext _dbContext;

        public CustomerRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public CustomerEntity? GetCustomerById(Guid id)
        {
            var response = GetByIdAsync(id).GetAwaiter().GetResult();
            return response;
        }

        public CustomerEntity? GetCustomerByEmail(Email email)
        {
            var response = FirstOrDefaultAsync(new CustomerWithEmailSpecification(email)).GetAwaiter().GetResult();
            return response;
        }
    }
}