using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.DTOs.Auth
{
    public record RefreshTokenDto(string JwtToken, string RefreshToken);
}
