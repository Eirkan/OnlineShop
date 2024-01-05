using Product.Domain.Entities.Users;

namespace Product.Application.Services.Authentication.Common
{
    public record AuthenticationResult(
        User User,
        string Token);
}