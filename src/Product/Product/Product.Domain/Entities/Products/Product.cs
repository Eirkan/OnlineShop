using Product.Core.Domain.Primitives;

namespace Product.Domain.Entities.Products;

public sealed class Product : AggregateRoot<Guid>
{
    public Product() : base(default) { }

    public Product(Guid id) : base(id)
    {
    }


    public string Name { get; init; } = null!;

    public string Description { get; init; } = null!;

    public decimal Price { get; set; }

    public int AvailableStock { get; set; }

}
