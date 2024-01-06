using Order.Core.Domain.Primitives;
using Order.Domain.Entities.OrderAggregate.Event;
using System.ComponentModel.DataAnnotations;

namespace Order.Domain.Entities.OrderAggregate;

public class Order : AggregateRoot<int>
{
    public DateTime OrderDate { get; private set; }

    [Required]
    public Address Address { get; private set; }

    public int? GetBuyerId => _buyerId;
    private int? _buyerId;

    public OrderStatus OrderStatus { get; private set; }
    private int _orderStatusId;

    private string _description;

    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    private int? _paymentMethodId;

    public Order() : base(default) { }

    public Order(string userId, string userName, Address address, int cardTypeId, string cardNumber, string cardSecurityNumber,
        string cardHolderName, DateTime cardExpiration, int? buyerId = null, int? paymentMethodId = null) : this()
    {
        _buyerId = buyerId;
        _paymentMethodId = paymentMethodId;
        _orderStatusId = OrderStatus.Submitted.Value;
        OrderDate = DateTime.UtcNow;
        Address = address;

        AddOrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
    }

    private void AddOrderStartedDomainEvent(string userId, string userName, int cardTypeId, string cardNumber,
        string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
    {
        var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName, cardTypeId,
                                                                    cardNumber, cardSecurityNumber,
                                                                    cardHolderName, cardExpiration);

        this.AddDomainEvent(orderStartedDomainEvent);
    }

}

