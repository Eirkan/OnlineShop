using Order.Core.Domain.Messaging.Events;

namespace Order.Domain.Entities.OrderAggregate.Event;

public sealed class OrderStartedDomainEvent : BaseDomainEvent
{
    public Order Order { get; }
    public string UserId { get; }
    public string UserName { get; }
    public int CardTypeId { get; }
    public string CardNumber { get; }
    public string CardSecurityNumber { get; }
    public string CardHolderName { get; }
    public DateTime CardExpiration { get; }

    public OrderStartedDomainEvent(
            Order Order,
            string UserId,
            string UserName,
            int CardTypeId,
            string CardNumber,
            string CardSecurityNumber,
            string CardHolderName,
            DateTime CardExpiration)
    {
        this.Order = Order;
        this.UserId = UserId;
        this.UserName = UserName;
        this.CardTypeId = CardTypeId;
        this.CardNumber = CardNumber;
        this.CardSecurityNumber = CardSecurityNumber;
        this.CardHolderName = CardHolderName;
        this.CardExpiration = CardExpiration;
    }
}
