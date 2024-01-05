using Mapster;
using Product.Application.Services.Authentication.Common;
using Product.Contracts.Authentication.Register;

namespace Product.Application.Authentication.Commands.Register
{
    public class RegisterMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AuthenticationResult, RegisterResponse>()
                .Map(dest => dest.Token, src => src.Token)
                .Map(dest => dest, src => src.User);
        }
    }
}
