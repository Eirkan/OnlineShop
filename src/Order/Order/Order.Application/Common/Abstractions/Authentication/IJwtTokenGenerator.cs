using Order.Domain.Entities.Users;

namespace Order.Application.Common.Abstractions.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}