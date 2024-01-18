using ErrorOr;
using Product.Core.Domain.Extensions;
using Product.Core.Domain.Primitives;
using Errors = Product.Domain.Common.Errors.Errors;

namespace Product.Domain.ValueObjects
{
    public sealed class ProductName : ValueObject
    {
        public const int MaxLength = 256;

        internal static readonly ProductName Empty = new ProductName();

        public string Value { get; private set; }


        private ProductName(string productName)
        {
            Value = productName;
        }

        private ProductName()
        {
            Value = string.Empty;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            return this.GetComponents<ProductName>();
        }

        public static ErrorOr<ProductName> Create(string? firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return Errors.ProductName.NullOrEmpty;
            }

            if (firstName.Length > MaxLength)
            {
                return Errors.ProductName.LongerThanAllowed;
            }


            return new ProductName(firstName);
        }

        public static implicit operator string(ProductName email) => email?.Value ?? string.Empty;

        public static explicit operator ProductName(string email)
        {
            ErrorOr<ProductName> firstNameResult = Create(email);
            ProductName? firstName = firstNameResult.Value;

            return firstNameResult.IsError || firstName is null ? Empty : firstName;
        }
    }
}
