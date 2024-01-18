using FluentValidation;

namespace Customer.Application.Customer.Commands.Insert
{
    public class InsertCommandValidator : AbstractValidator<InsertCommand>
    {
        public InsertCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
