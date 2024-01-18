using Customer.Contracts.Customer.Insert;
using Mapster;
using CustomerEntity = Customer.Domain.Entities.Customers.Customer;

namespace Customer.Application.Customer.Commands.Insert
{
    public class InsertMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CustomerEntity, InsertResponse>()
                .Map(dest => dest.CustomerId, src => src.Id)
                .Map(dest => dest, src => src);
        }
    }
}
