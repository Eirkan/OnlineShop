using ErrorOr;

namespace Customer.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class FirstName
        {
            public static Error NullOrEmpty => Error.Validation("FirstName.NullOrEmpty", "The first name is required.");

            public static Error LongerThanAllowed => Error.Validation("FirstName.LongerThanAllowed", "The first name is longer than allowed.");
        }
    }
}
