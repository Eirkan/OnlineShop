using Customer.Domain.Repositories;
using Customer.Domain.ValueObjects;
using Customer.Infrastructure.Persistence.Specifications;

namespace Customer.Infrastructure.Persistence.Repositories
{
    internal sealed class CustomerRepository : GenericRepository<Domain.Entities.Customers.Customer>, ICustomerRepository
    {
        private readonly IDbContext _dbContext;

        public CustomerRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public Domain.Entities.Customers.Customer? GetCustomerById(Guid id)
        {
            var response = GetByIdAsync(id).GetAwaiter().GetResult();
            return response;
        }

        public Domain.Entities.Customers.Customer? GetCustomerByEmail(Email email)
        {
            var response = FirstOrDefaultAsync(new CustomerWithEmailSpecification(email)).GetAwaiter().GetResult();
            return response;
        }
    }
}