using AutoAukcionas.Auth;
using AutoAukcionas.Auth.Model;
using AutoAukcionas.Data.DTOs.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace AutoAukcionas.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AuctionUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;
        private readonly IRefreshToken _refreshToken;
        public AuthController(UserManager<AuctionUser> userManager, IMapper mapper, ITokenManager tokenManager, IRefreshToken refreshToken)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenManager = tokenManager;
            _refreshToken = refreshToken;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            List<string> errors = new List<string>();
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null) return BadRequest("This email is already taken!");

            var newUser = new AuctionUser
            {
                Email = dto.Email,
                UserName = dto.Username,
                Name = dto.Name,
                Surname = dto.Surname
            };

            var createdUserResult = await _userManager.CreateAsync(newUser, dto.Password);
            if (!createdUserResult.Succeeded)
            {
                foreach (var error in createdUserResult.Errors)
                {
                    errors.Add(error.Description);
                }

                var json = JsonSerializer.Serialize(errors);

                return BadRequest(json);
            }

            if (dto.Type == "Buyer")
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Buyer);
                return CreatedAtAction(nameof(Register), _mapper.Map<UserDto>(newUser));
            }

            if(dto.Type == "Seller")
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Seller);
                return CreatedAtAction(nameof(Register), _mapper.Map<UserDto>(newUser));
            }
            else
            {
                return BadRequest("Could not create user!");
            }
            
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return BadRequest("Username or password is invalid!");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if(!isPasswordValid) return BadRequest("Username or password is invalid!");

            var roles = await _userManager.GetRolesAsync(user);
            var listroles = roles.ToList<string>();

            var accessToken = await _tokenManager.CreateAccessTokenAsyc(user);

            return Ok(new LoggedInDto(accessToken.JWTToken, accessToken.RefreshToken, listroles, user.UserName, user.Id));

        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            var token = await _refreshToken.Refresh(dto);
            if (token == null) return BadRequest();

            return Ok(token);

        }
    }
}
