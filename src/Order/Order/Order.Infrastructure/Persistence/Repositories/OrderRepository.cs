
using Microsoft.EntityFrameworkCore;
using Order.Domain.Repositories;
using Order.Domain.ValueObjects;
using Order.Infrastructure.Persistence.Specifications;

namespace Order.Infrastructure.Persistence.Repositories;

internal sealed class OrderRepository : GenericRepository<Domain.Entities.OrderAggregate.Order>, IOrderRepository
{
    private readonly IDbContext _dbContext;

    public OrderRepository(IDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Domain.Entities.OrderAggregate.Order> GetOrdersByDate(DateTime startDate, DateTime endDate)
    {
        var response = EntityModel.Include(ci => ci.OrderItems)
            .Where(q => q.OrderDate >= startDate && q.OrderDate <= endDate)
            .ToList();

        return response;
    }

    public Domain.Entities.OrderAggregate.Order? GetOrderById(int id)
    {
        var response = GetByIdAsync(id).GetAwaiter().GetResult();
        return response;
    }
}