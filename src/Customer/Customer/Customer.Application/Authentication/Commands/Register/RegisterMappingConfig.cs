using Customer.Application.Services.Authentication.Common;
using Customer.Contracts.Authentication.Register;
using Mapster;

namespace Customer.Application.Authentication.Commands.Register
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
