using Product.Domain.Repositories;

namespace Product.Infrastructure.Persistence.Repositories
{
    internal sealed class ProductRepository : GenericRepository<Domain.Entities.Products.Product>, IProductRepository
    {
        private readonly IDbContext _dbContext;

        public ProductRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Domain.Entities.Products.Product? GetOrderById(int id)
        {
            var response = GetByIdAsync(id).GetAwaiter().GetResult();
            return response;
        }

        public void Insert(Domain.Entities.Products.Product product)
        {
            Insert(product);
        }

        public void Update(Domain.Entities.Products.Product product)
        {
            Update(product);
        }
    }
}