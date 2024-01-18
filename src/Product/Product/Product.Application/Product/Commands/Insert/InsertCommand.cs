using ErrorOr;
using Product.Contracts.Product.Insert;
using Product.Core.Domain.Messaging;

namespace Product.Application.Product.Commands.Insert
{
    public record InsertCommand(
        string Name,
        string Description,
        decimal Price,
        int AvailableStock) : ICommand<ErrorOr<InsertResponse>>;

}
