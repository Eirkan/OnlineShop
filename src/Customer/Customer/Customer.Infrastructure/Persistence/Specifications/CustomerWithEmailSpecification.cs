using Customer.Core.Specification;
using Customer.Domain.ValueObjects;
using System.Linq.Expressions;

namespace Customer.Infrastructure.Persistence.Specifications
{
    /// <summary>
    /// Represents the specification for determining the customer with email.
    /// </summary>
    public sealed class CustomerWithEmailSpecification : Specification<Domain.Entities.Customers.Customer>
    {
        private readonly Email _email;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerWithEmailSpecification"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        internal CustomerWithEmailSpecification(Email email) => _email = email;

        /// <inheritdoc />
        public override Expression<Func<Domain.Entities.Customers.Customer, bool>> ToExpression() => customer => customer.Email == _email.Value;
    }
}
