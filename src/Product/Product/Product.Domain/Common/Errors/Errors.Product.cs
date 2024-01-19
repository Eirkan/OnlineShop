using ErrorOr;

namespace Product.Domain.Common.Errors;

public static partial class Errors
{
    public static class Product
    {
        public static Error NullOrEmpty => Error.Validation(code: "Product.NullOrEmpty", description: "The Product is null.");

        public static Error IdNullOrEmpty => Error.Validation(code: "ProductId.NullOrEmpty", description: "The Product Id is null.");
    }
}
