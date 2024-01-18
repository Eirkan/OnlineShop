using Customer.Contracts.Customer.Insert;
using Customer.Core.Domain.Messaging;
using ErrorOr;

namespace Customer.Application.Customer.Commands.Insert
{
    public record InsertCommand(
        string FirstName,
        string LastName,
        string Email) : ICommand<ErrorOr<InsertResponse>>;

}
