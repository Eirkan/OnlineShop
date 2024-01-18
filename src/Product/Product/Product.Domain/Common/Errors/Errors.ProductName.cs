using ErrorOr;

namespace Product.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class ProductName
        {
            public static Error NullOrEmpty => Error.Validation("ProductName.NullOrEmpty", "The product name is required.");

            public static Error LongerThanAllowed => Error.Validation("ProductName.LongerThanAllowed", "The product name is longer than allowed.");
        }
    }
}
