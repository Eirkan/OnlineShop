using Mapster;
using Order.Application.Services.Authentication.Common;
using Order.Contracts.Authentication.Register;

namespace Order.Application.Authentication.Commands.Register
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
