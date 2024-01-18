using Microsoft.EntityFrameworkCore;

namespace Product.Infrastructure.Persistence
{
    public class ProductDbContext : DbContext, IDbContext
    {
        private readonly string _defaultSchemaName = string.Empty;


        public DbSet<Domain.Entities.Products.Product> Products { get; set; }


        public new DbSet<TEntity> Set<TEntity>()
            where TEntity : class
            => base.Set<TEntity>();

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }


        public async Task<TEntity> GetBydIdAsync<TEntity>(object id) where TEntity : class
        {
            return Set<TEntity>().Find(id);
        }


        public void Insert<TEntity>(TEntity entity)
            where TEntity : class
            => Set<TEntity>().Add(entity);


        public void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
            where TEntity : class
            => Set<TEntity>().AddRange(entities);


        /// <inheritdoc />
        public new void Remove<TEntity>(TEntity entity)
            where TEntity : class
            => Set<TEntity>().Remove(entity);


        public Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                DbContextConfigurer.Configure(optionsBuilder);
                base.OnConfiguring(optionsBuilder);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //if (!string.IsNullOrWhiteSpace(_defaultSchemaName))
            //{
            //    modelBuilder.HasDefaultSchema(_defaultSchemaName);
            //}
        }

    }
}