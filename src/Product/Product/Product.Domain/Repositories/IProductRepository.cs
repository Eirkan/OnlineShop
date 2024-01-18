using Product.Core.Common.Dependency;
using Product.Domain.Entities.Products;
using Product.Domain.ValueObjects;

namespace Product.Domain.Repositories
{
    public interface IProductRepository : IScopedDependency
    {
        Entities.Products.Product? GetOrderById(int id);

        void Insert(Entities.Products.Product product);

        void Update(Entities.Products.Product product);
    }
}
