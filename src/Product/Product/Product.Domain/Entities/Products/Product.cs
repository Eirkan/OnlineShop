using Product.Core.Domain.Primitives;
using Product.Domain.Entities.Products.Events;

namespace Product.Domain.Entities.Products;

public sealed class Product : AggregateRoot<Guid>
{
    public Product() : base(default) { }
    public Product(Guid id, string name,
        string description, decimal price, int availableStock) : base(id)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        AvailableStock = availableStock;
    }


    public string Name { get; init; } = null!;

    public string Description { get; init; } = null!;

    public decimal Price { get; set; }

    public int AvailableStock { get; set; }


    public void InsertProduct()
    {
        var @event = GetProductCreatedDomainEvent();
        AddDomainEvent(@event);
    }

    public ProductCreatedDomainEvent GetProductCreatedDomainEvent()
    {
        return new ProductCreatedDomainEvent(Id, Name, Description, Price, AvailableStock);
    }
}
