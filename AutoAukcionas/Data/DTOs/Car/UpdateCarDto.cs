using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AutoAukcionas.Data.DTOs.Car
{
    public record UpdateCarDto([Required]string Year, [Required]string Fuel_type, [Required]float Litrage);
}
