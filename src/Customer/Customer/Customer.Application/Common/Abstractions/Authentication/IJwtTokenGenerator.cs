using Customer.Domain.Entities.Customers;

namespace Customer.Application.Common.Abstractions.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Domain.Entities.Customers.Customer user);
    }
}