using Microsoft.EntityFrameworkCore;
using Product.Core.Specification;

namespace Product.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Represents the generic repository with the most common repository methods.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    internal abstract class GenericRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        protected GenericRepository(IDbContext dbContext)
        {
            DbContext = dbContext;
            EntityModel = dbContext.Set<TEntity>();
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        protected IDbContext DbContext { get; }
        protected DbSet<TEntity> EntityModel { get; }

        /// <summary>
        /// Gets the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <returns>The maybe instance that may contain the entity with the specified identifier.</returns>
        public async Task<TEntity> GetByIdAsync(object id) => await DbContext.GetBydIdAsync<TEntity>(id);

        /// <summary>
        /// Inserts the specified entity into the database.
        /// </summary>
        /// <param name="entity">The entity to be inserted into the database.</param>
        public void Insert(TEntity entity) => DbContext.Insert(entity);

        /// <summary>
        /// Inserts the specified entities to the database.
        /// </summary>
        /// <param name="entities">The entities to be inserted into the database.</param>
        public void InsertRange(IReadOnlyCollection<TEntity> entities) => DbContext.InsertRange(entities);

        /// <summary>
        /// Updates the specified entity in the database.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        public void Update(TEntity entity) => DbContext.Set<TEntity>().Update(entity);

        /// <summary>
        /// Removes the specified entity from the database.
        /// </summary>
        /// <param name="entity">The entity to be removed from the database.</param>
        public void Remove(TEntity entity) => DbContext.Remove(entity);

        /// <summary>
        /// Checks if any entity meets the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>True if any entity meets the specified specification, otherwise false.</returns>
        protected async Task<bool> AnyAsync(Specification<TEntity> specification) =>
            await DbContext.Set<TEntity>().AnyAsync(specification);

        /// <summary>
        /// Gets the first entity that meets the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The maybe instance that may contain the first entity that meets the specified specification.</returns>
        protected async Task<TEntity?> FirstOrDefaultAsync(Specification<TEntity> specification) =>
            await DbContext.Set<TEntity>().FirstOrDefaultAsync(specification);

        protected async Task<List<TEntity>> ToListAsync(Specification<TEntity> specification) =>
            await DbContext.Set<TEntity>().Where(specification).ToListAsync();
    }
}