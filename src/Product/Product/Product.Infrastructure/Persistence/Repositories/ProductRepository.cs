using Product.Domain.Repositories;
using ProductEntity = Product.Domain.Entities.Products.Product;

namespace Product.Infrastructure.Persistence.Repositories;

internal sealed class ProductRepository : GenericRepository<ProductEntity>, IProductRepository
{
    private readonly IDbContext _dbContext;

    public ProductRepository(IDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public ProductEntity? GetProductId(Guid id)
    {
        var response = GetByIdAsync(id).GetAwaiter().GetResult();
        return response;
    }
}