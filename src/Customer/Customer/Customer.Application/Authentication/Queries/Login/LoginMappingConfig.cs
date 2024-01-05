using Customer.Application.Services.Authentication.Common;
using Customer.Contracts.Authentication.Login;
using Customer.Domain.Entities.Users;
using Mapster;

namespace Customer.Application.Authentication.Queries.Login
{
    public class LoginMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AuthenticationResult, LoginResponse>()
                .Map(dest => dest.Token, src => src.Token)
                .Map(dest => dest, src => src.User);

            config.NewConfig<User, LoginResponse>()
                .Map(dest => dest, src => src);
        }
    }
}
