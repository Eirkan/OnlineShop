using Customer.Core.Domain.Primitives;
using Customer.Domain.Entities.Customers.Events;

namespace Customer.Domain.Entities.Customers
{
    public sealed class Customer : AggregateRoot<Guid>
    {
        public Customer() : base(default) { }

        public Customer(Guid id
            , string firstName
            , string lastName
            , string email
            )
            : base(id)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string Email { get; init; } = null!;

        public void InsertCustomer()
        {
            var @event = GetCustomerCreatedDomainEvent();
            AddDomainEvent(@event);
        }

        public CustomerCreatedDomainEvent GetCustomerCreatedDomainEvent()
        {
            return new CustomerCreatedDomainEvent(Id, FirstName, LastName, Email);
        }
    }
}