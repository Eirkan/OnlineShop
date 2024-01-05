using Customer.Application.Services.Authentication.Common;
using Customer.Core.Domain.Messaging;
using ErrorOr;

namespace Customer.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password) : ICommand<ErrorOr<AuthenticationResult>>;

}
