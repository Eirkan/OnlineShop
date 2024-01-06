namespace Order.Contracts.Order.GetOrdersByDateRange;

public record GetOrdersByDateRangeRequest(DateTime StartDate, DateTime EndDate);
