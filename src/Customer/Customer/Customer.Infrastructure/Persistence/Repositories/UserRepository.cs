using Customer.Domain.Entities.Customers;
using Customer.Domain.Repositories;
using Customer.Domain.ValueObjects;
using Customer.Infrastructure.Persistence.Specifications;

namespace Customer.Infrastructure.Persistence.Repositories
{
    internal sealed class UserRepository : GenericRepository<Domain.Entities.Customers.Customer>, ICustomerRepository
    {
        private readonly IDbContext _dbContext;
        //protected readonly DbSet<User> Users;

        public UserRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
            //Users = dbContext.Set<User>();
        }


        public Domain.Entities.Customers.Customer? GetUserById(Guid id)
        {
            //var response = _dbContext.Users.Where(SpecificationByGuidId<User>.Create(id)).SingleOrDefault();
            var response = GetByIdAsync(id).GetAwaiter().GetResult();
            return response;
        }

        public Domain.Entities.Customers.Customer? GetCustomerByEmail(Email email)
        {
            var response = FirstOrDefaultAsync(new UserWithEmailSpecification(email)).GetAwaiter().GetResult();
            return response;
        }
    }
}