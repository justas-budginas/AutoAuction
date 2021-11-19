using AutoAukcionas.Auth.Model;
using AutoAukcionas.Data.DTOs.Auth;
using AutoAukcionas.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoAukcionas.Auth
{
    public interface ITokenManager
    {
        Task<AuthResponse> CreateAccessTokenAsyc(AuctionUser user);
        Task<AuthResponse> CreateAccessTokenAsyc(string username, IEnumerable<Claim> claims);
    }

    public class TokenManager : ITokenManager
    {
        private readonly SymmetricSecurityKey _authSignInKey;
        private readonly UserManager<AuctionUser> _userManager;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshToken;
        public TokenManager(IConfiguration configuration, UserManager<AuctionUser> manager, IRefreshTokenGenerator refreshTokenGenerator, IRefreshTokenRepository refreshToken)
        {
            _authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            _userManager = manager;
            _issuer = configuration["JWT:ValidIssuer"];
            _audience = configuration["JWT:ValidAudience"];
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshToken = refreshToken;
        }

        public async Task<AuthResponse> CreateAccessTokenAsyc(string username, IEnumerable<Claim> claims)
        {
            var accessSecurityToken = new JwtSecurityToken
                (
                    issuer: _issuer,
                    audience: _audience,
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(_authSignInKey, SecurityAlgorithms.HmacSha256)
                );

            var token = new JwtSecurityTokenHandler().WriteToken(accessSecurityToken);
            var refreshToken = _refreshTokenGenerator.GenerateToken();

            if (await _refreshToken.Exist(username))
            {
                await _refreshToken.Update(new Data.Entities.RefreshToken { Username = username, Refreshtoken = refreshToken });
            }
            else
            {
                await _refreshToken.Create(new Data.Entities.RefreshToken { Username = username, Refreshtoken = refreshToken });
            }

            return new AuthResponse { JWTToken = token, RefreshToken = refreshToken };

        }

        public async Task<AuthResponse> CreateAccessTokenAsyc(AuctionUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(CustomClaims.UserId, user.Id.ToString())
            };
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var accessSecurityToken = new JwtSecurityToken
                (
                    issuer: _issuer,
                    audience: _audience,
                    expires:DateTime.UtcNow.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(_authSignInKey, SecurityAlgorithms.HmacSha256)
                );

            var token = new JwtSecurityTokenHandler().WriteToken(accessSecurityToken);
            var refreshToken = _refreshTokenGenerator.GenerateToken();

            if(await _refreshToken.Exist(user.UserName))
            {
                await _refreshToken.Update(new Data.Entities.RefreshToken { Username = user.UserName, Refreshtoken = refreshToken });
            }
            else
            {
                await _refreshToken.Create(new Data.Entities.RefreshToken { Username = user.UserName, Refreshtoken = refreshToken });
            }

            return new AuthResponse { JWTToken = token, RefreshToken = refreshToken };
        }
    }
}
