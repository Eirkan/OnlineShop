using ErrorOr;
using Order.Core.Domain.Extensions;
using Order.Core.Domain.Primitives;
using Errors = Order.Domain.Common.Errors.Errors;

namespace Order.Domain.ValueObjects
{
    public sealed class FirstName : ValueObject
    {
        public const int MaxLength = 256;

        internal static readonly FirstName Empty = new FirstName();

        public string Value { get; private set; }


        private FirstName(string email)
        {
            Value = email;
        }

        private FirstName()
        {
            Value = string.Empty;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            return this.GetComponents<FirstName>();
        }

        public static ErrorOr<FirstName> Create(string? firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return Errors.FirstName.NullOrEmpty;
            }

            if (firstName.Length > MaxLength)
            {
                return Errors.FirstName.LongerThanAllowed;
            }


            return new FirstName(firstName);
        }

        public static implicit operator string(FirstName email) => email?.Value ?? string.Empty;

        public static explicit operator FirstName(string email)
        {
            ErrorOr<FirstName> firstNameResult = Create(email);
            FirstName? firstName = firstNameResult.Value;

            return firstNameResult.IsError || firstName is null ? Empty : firstName;
        }
    }
}
