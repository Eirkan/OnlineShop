using Customer.Core.Specification;
using Customer.Domain.Entities.Users;
using Customer.Domain.ValueObjects;
using System.Linq.Expressions;

namespace Customer.Infrastructure.Persistence.Specifications
{
    /// <summary>
    /// Represents the specification for determining the user with email.
    /// </summary>
    public sealed class UserWithEmailSpecification : Specification<User>
    {
        private readonly Email _email;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserWithEmailSpecification"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        internal UserWithEmailSpecification(Email email) => _email = email;

        /// <inheritdoc />
        public override Expression<Func<User, bool>> ToExpression() => user => user.Email == _email.Value;
    }
}
