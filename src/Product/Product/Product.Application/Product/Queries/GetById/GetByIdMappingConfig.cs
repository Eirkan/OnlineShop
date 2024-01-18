using Mapster;
using Product.Contracts.Product.GetById;

namespace Product.Application.Product.Queries.GetById
{
    public class GetByIdMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Products.Product, GetByIdResponse>()
                .Map(dest => dest, src => src);
        }
    }
}
