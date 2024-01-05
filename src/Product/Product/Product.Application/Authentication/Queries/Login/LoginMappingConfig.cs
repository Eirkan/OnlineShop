using Mapster;
using Product.Application.Services.Authentication.Common;
using Product.Contracts.Authentication.Login;
using Product.Domain.Entities.Users;

namespace Product.Application.Authentication.Queries.Login
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
