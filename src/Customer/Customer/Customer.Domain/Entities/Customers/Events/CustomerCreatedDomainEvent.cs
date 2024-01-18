using Customer.Core.Domain.Messaging.Events;

namespace Customer.Domain.Entities.Customers.Events
{
    /// <summary>
    /// Represents the event that fires when a Customer is created.
    /// </summary>
    public sealed class CustomerCreatedDomainEvent : BaseDomainEvent
    {
        public CustomerCreatedDomainEvent(Guid userId, string firstName, string lastName, string email)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }


        public Guid UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
    }
}
