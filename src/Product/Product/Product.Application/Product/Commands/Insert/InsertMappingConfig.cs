using Mapster;
using Product.Contracts.Product.Insert;
using ProductEntity = Product.Domain.Entities.Products.Product;

namespace Product.Application.Product.Commands.Insert;

public class InsertMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductEntity, InsertResponse>()
            .Map(dest => dest, src => src);
    }
}
