using FluentValidation;

namespace Product.Application.Product.Commands.Insert
{
    public class InsertCommandValidator : AbstractValidator<InsertCommand>
    {
        public InsertCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.AvailableStock).NotEmpty();
        }
    }
}
