using ErrorOr;
using Product.Application.Services.Authentication.Common;
using Product.Core.Domain.Messaging;

namespace Product.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password) : ICommand<ErrorOr<AuthenticationResult>>;

}
