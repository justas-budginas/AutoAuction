using AutoAukcionas.Auth.Model;
using AutoAukcionas.Data.DTOs.Auth;
using AutoAukcionas.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace AutoAukcionas.Auth
{
    public interface IRefreshToken
    {
        Task<AuthResponse> Refresh(RefreshTokenDto dto);
    }

    public class RefreshToken : IRefreshToken
    {
        private readonly IConfiguration _cofiguration;
        private readonly IRefreshTokenRepository _refreshToken;
        private readonly ITokenManager _tokenManager;

        public RefreshToken(IConfiguration configuration, IRefreshTokenRepository refreshToken, ITokenManager tokenManager)
        {
            _cofiguration = configuration;
            _refreshToken = refreshToken;
            _tokenManager = tokenManager;
        }

        public async Task<AuthResponse> Refresh(RefreshTokenDto dto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken ValidatedToken;
            var validate = tokenHandler.ValidateToken(dto.JwtToken,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cofiguration["JWT:Secret"])),
                    ValidateAudience = false,
                    ValidateIssuer = false

                }, out ValidatedToken);
            var JwtToken = ValidatedToken as JwtSecurityToken;

            if(JwtToken == null || !JwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                throw new SecurityTokenException("Invalid Token passed!");
            }

            var username = validate.Identity.Name;
            var dbUsernameToken = await _refreshToken.Get(username);

            if(dto.RefreshToken != dbUsernameToken)
            {
                throw new SecurityTokenException("Invalid RefreshToken passed!");
            }

            return await _tokenManager.CreateAccessTokenAsyc(username, validate.Claims);
        }
    }
}
