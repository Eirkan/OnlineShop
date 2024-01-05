using ErrorOr;

namespace Product.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Email
        {
            public static Error NullOrEmpty => Error.Validation(code: "Email.NullOrEmpty", description: "The email is required.");

            public static Error LongerThanAllowed => Error.Validation(code: "Email.LongerThanAllowed", description: "The email is longer than allowed.");

            public static Error InvalidFormat => Error.Validation(code: "Email.InvalidFormat", description: "The email format is invalid.");
        }
    }
}
