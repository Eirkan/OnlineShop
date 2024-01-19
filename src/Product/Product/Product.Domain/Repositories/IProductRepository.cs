using Product.Core.Common.Dependency;
using ProductEntity = Product.Domain.Entities.Products.Product;

namespace Product.Domain.Repositories;

public interface IProductRepository : IScopedDependency
{
    ProductEntity? GetProductId(Guid id);

    void Insert(ProductEntity product);

    void Update(ProductEntity product);
}
