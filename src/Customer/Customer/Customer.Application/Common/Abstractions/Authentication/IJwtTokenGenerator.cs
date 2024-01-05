using Customer.Domain.Entities.Users;

namespace Customer.Application.Common.Abstractions.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}