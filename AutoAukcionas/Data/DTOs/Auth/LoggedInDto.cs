using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.DTOs.Auth
{
    public record LoggedInDto(string AccesToken, string RefreshToken, List<string> roles, string username, string userid);
}
