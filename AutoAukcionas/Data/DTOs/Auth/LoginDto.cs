using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.DTOs.Auth
{
    public record LoginDto([EmailAddress][Required]string Email, [Required]string Password);
}
