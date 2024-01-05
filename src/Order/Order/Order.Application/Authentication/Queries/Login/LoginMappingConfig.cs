using Mapster;
using Order.Application.Services.Authentication.Common;
using Order.Contracts.Authentication.Login;
using Order.Domain.Entities.Users;

namespace Order.Application.Authentication.Queries.Login
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
