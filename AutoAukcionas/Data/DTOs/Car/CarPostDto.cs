using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AutoAukcionas.Data.DTOs.Car
{
    public record CarPostDto([Required]string Make, [Required]string Model, [Required]string Year, [Required] string Fuel_type,
       [Required]float Litrage, [Required]float Starting_price, [Required]IFormFile CarImage);
}
