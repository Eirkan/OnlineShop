using Customer.Contracts.Customer.GetByEMail;
using Mapster;
using CustomerEntity = Customer.Domain.Entities.Customers.Customer;

namespace Customer.Application.Customer.Queries.GetByEMail
{
    public class GetByEMailMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CustomerEntity, GetByEMailResponse>()
                .Map(dest => dest.CustomerId, src => src.Id)
                .Map(dest => dest, src => src);
        }
    }
}
