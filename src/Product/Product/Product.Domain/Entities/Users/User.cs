using Product.Core.Domain.Primitives;
using Product.Domain.Entities.Users.Events;

namespace Product.Domain.Entities.Users
{
    public sealed class User : AggregateRoot<Guid>
    {
        public User() : base(default) { }

        public User(Guid id
            , string firstName
            , string lastName
            , string email
            , string password
            )
            : base(id)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }

        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string Email { get; init; } = null!;

        public string Password { get; init; } = null!;

        public void RegisterUser()
        {
            var @event = GetUserCreatedDomainEvent();
            this.AddDomainEvent(@event);
        }

        public UserCreatedDomainEvent GetUserCreatedDomainEvent()
        {
            return new UserCreatedDomainEvent(this.Id, this.FirstName, this.LastName, this.Email);
        }
    }
}