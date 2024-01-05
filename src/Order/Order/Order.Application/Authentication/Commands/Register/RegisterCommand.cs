using ErrorOr;
using Order.Application.Services.Authentication.Common;
using Order.Core.Domain.Messaging;

namespace Order.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password) : ICommand<ErrorOr<AuthenticationResult>>;

}
