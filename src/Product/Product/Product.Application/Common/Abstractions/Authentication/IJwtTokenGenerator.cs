using Product.Domain.Entities.Users;

namespace Product.Application.Common.Abstractions.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}