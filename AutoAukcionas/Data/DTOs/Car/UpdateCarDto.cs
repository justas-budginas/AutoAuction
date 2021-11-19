using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.DTOs.Car
{
    public record UpdateCarDto([Required]string Year, [Required]string Fuel_type, [Required]float Litrage);
}
