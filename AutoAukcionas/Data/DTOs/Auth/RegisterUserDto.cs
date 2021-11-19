using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.DTOs.Auth
{
    public record RegisterUserDto([Required]string Username, [Required]string Name, [Required]string Surname, [EmailAddress][Required]string Email,
        [Required]string Password, [Required]string Type);
}
