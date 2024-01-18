using Mapster;
using Product.Contracts.Product.Insert;

namespace Product.Application.Product.Commands.Insert
{
    public class InsertMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Products.Product, InsertResponse>()
                .Map(dest => dest, src => src);
        }
    }
}
