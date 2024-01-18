using ErrorOr;

namespace Customer.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Customer
        {
            public static Error NullOrEmpty => Error.Validation(code: "Customer.NullOrEmpty", description: "The Customer is null.");

            public static Error DuplicateEmail => Error.Conflict(
                code: "Customer.DuplicateEmail",
                description: "Email already in customer");
        }
    }
}
