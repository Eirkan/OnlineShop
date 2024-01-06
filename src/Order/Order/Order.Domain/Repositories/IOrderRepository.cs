using Order.Core.Common.Dependency;
using Order.Domain.ValueObjects;

namespace Order.Domain.Repositories;

public interface IOrderRepository : IScopedDependency
{
    Entities.OrderAggregate.Order? GetOrderById(int id);
    IEnumerable<Entities.OrderAggregate.Order> GetOrdersByDate(DateTime startDate, DateTime endDate);

    void Insert(Entities.OrderAggregate.Order user);

    void Update(Entities.OrderAggregate.Order user);
}
