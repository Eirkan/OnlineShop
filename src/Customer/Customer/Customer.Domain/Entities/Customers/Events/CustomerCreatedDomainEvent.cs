using Customer.Core.Domain.Messaging.Events;

namespace Customer.Domain.Entities.Customers.Events
{
    /// <summary>
    /// Represents the event that fires when a Customer is created.
    /// </summary>
    public sealed class CustomerCreatedDomainEvent : BaseDomainEvent
    {
        public CustomerCreatedDomainEvent(Guid customerId, string firstName, string lastName, string email)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }


        public Guid CustomerId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
    }
}
