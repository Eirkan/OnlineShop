using Order.Core.Domain.Messaging.Events;

namespace Order.Contracts.Orders.IntegrationEvents;
public record OrderStartedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public string OrderStatus { get; }
    public string BuyerName { get; }
    public string BuyerIdentityGuid { get; }

    public OrderStartedIntegrationEvent(int orderId, string orderStatus, string buyerName, string buyerIdentityGuid)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
        BuyerIdentityGuid = buyerIdentityGuid;
    }
}
