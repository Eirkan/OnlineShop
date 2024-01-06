namespace Order.Contracts.Order.GetOrdersByDateRange;

public record GetOrdersByDateRangeResponse(
    int OrderId,
    DateTime OrderDate);
