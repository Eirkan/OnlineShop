using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Product.Application.Common.Abstractions.Authentication;
using Product.Domain.Entities.Users;
using Product.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Product.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
        {
            _dateTimeProvider = dateTimeProvider;
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(User user)
        {
            var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256
            );
            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}