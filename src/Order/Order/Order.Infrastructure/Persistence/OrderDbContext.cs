using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities.OrderAggregate;
using Order.Infrastructure.Persistence.EntityConfigurations;
using OrderAgg = Order.Domain.Entities.OrderAggregate.Order;

namespace Order.Infrastructure.Persistence
{
    public class OrderDbContext : DbContext, IDbContext
    {
        private readonly string _defaultSchemaName = string.Empty;


        public DbSet<OrderAgg> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Address> Addresses { get; set; }


        public new DbSet<TEntity> Set<TEntity>()
            where TEntity : class
            => base.Set<TEntity>();

        public OrderDbContext(DbContextOptions<OrderDbContext> options)
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

            if (!string.IsNullOrWhiteSpace(_defaultSchemaName))
            {
                modelBuilder.HasDefaultSchema(_defaultSchemaName);
            }

            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusEntityTypeConfiguration());

        }

    }
}