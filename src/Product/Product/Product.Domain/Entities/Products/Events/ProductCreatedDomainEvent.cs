using Product.Core.Domain.Messaging.Events;

namespace Product.Domain.Entities.Products.Events
{
    /// <summary>
    /// Represents the event that fires when a product is created.
    /// </summary>
    public sealed class ProductCreatedDomainEvent : BaseDomainEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }
        public int AvailableStock { get; }

        public ProductCreatedDomainEvent(Guid id, string name, string description, decimal price, int availableStock)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            AvailableStock = availableStock;
        }
    }
}
