using ErrorOr;
using Product.Core.Domain.Extensions;
using Product.Core.Domain.Primitives;
using System.Text.RegularExpressions;
using Errors = Product.Domain.Common.Errors.Errors;

namespace Product.Domain.ValueObjects
{
    /// <summary>
    /// Represents an email.
    /// </summary>
    public sealed class Email : ValueObject
    {

        public override IEnumerable<object> GetEqualityComponents()
        {
            //yield return Value;

            return this.GetComponents<Email>();
        }

        /// <summary>
        /// Returns an empty email object.
        /// </summary>
        internal static readonly Email Empty = new Email();

        public const int MaxLength = 256;

        private const string EmailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        private static readonly Lazy<Regex> EmailFormatRegex =
            new Lazy<Regex>(() => new Regex(EmailRegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));

        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class with the specified value.
        /// </summary>
        /// <param name="email">The email value.</param>
        private Email(string email)
        {
            Value = email;
        }

        private Email()
        {
            Value = string.Empty;
        }

        /// <summary>
        /// Gets the email value.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Creates a new <see cref="Email"/> instance with the specified email.
        /// </summary>
        /// <param name="email">The email value.</param>
        /// <returns>A new <see cref="Email"/> instance if the email is valid, or an error result.</returns>
        public static ErrorOr<Email> Create(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Errors.Email.NullOrEmpty;
            }

            if (email.Length > MaxLength)
            {
                return Errors.Email.LongerThanAllowed;
            }

            if (!EmailFormatRegex.Value.IsMatch(email))
            {
                return Errors.Email.InvalidFormat;
            }

            return new Email(email);
        }

        public static implicit operator string(Email email) => email?.Value ?? string.Empty;

        public static explicit operator Email(string email)
        {
            ErrorOr<Email> emailResult = Create(email);
            Email? emailValue = emailResult.Value;

            return emailResult.IsError || emailValue is null ? Empty : emailValue;
        }

        /// <inheritdoc />
    }
}
