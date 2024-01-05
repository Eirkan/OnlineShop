using Customer.Domain.Entities.Users;

namespace Customer.Application.Services.Authentication.Common
{
    public record AuthenticationResult(
        User User,
        string Token);
}