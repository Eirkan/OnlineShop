using ErrorOr;
using Order.Core.Domain.Messaging;

namespace Order.Application.Order.Queries.Login;

public record GetOrdersByDateRangeQuery(
    DateTime startDate,
    DateTime endDate) : IQuery<ErrorOr<List<Contracts.Order.GetOrdersByDateRange.GetOrdersByDateRangeResponse>>>;