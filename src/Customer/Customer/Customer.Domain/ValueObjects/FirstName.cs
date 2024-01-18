using Customer.Core.Domain.Extensions;
using Customer.Core.Domain.Primitives;
using ErrorOr;
using Errors = Customer.Domain.Common.Errors.Errors;

namespace Customer.Domain.ValueObjects
{
    public sealed class FirstName : ValueObject
    {
        public const int MaxLength = 256;

        internal static readonly FirstName Empty = new FirstName();

        public string Value { get; private set; }


        private FirstName(string firstName)
        {
            Value = firstName;
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

        public static explicit operator FirstName(string value)
        {
            ErrorOr<FirstName> firstNameResult = Create(value);
            FirstName? firstName = firstNameResult.Value;

            return firstNameResult.IsError || firstName is null ? Empty : firstName;
        }
    }
}
