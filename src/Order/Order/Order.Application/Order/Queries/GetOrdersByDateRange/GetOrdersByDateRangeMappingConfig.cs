using Mapster;
using Order.Contracts.Order.GetOrdersByDateRange;

namespace Order.Application.Order.Queries.Login
{
    public class GetOrdersByDateRangeMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<global::Order.Domain.Entities.OrderAggregate.Order, GetOrdersByDateRangeResponse>()
                //.Map(dest => dest.OrderDate, src => src.OrderDate)
                .Map(dest => dest, src => src);
        }
    }
}
