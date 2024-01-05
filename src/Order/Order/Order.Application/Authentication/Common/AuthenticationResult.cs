using Order.Domain.Entities.Users;

namespace Order.Application.Services.Authentication.Common
{
    public record AuthenticationResult(
        User User,
        string Token);
}