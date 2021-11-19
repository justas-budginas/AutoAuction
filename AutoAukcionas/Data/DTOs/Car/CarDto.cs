using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.DTOs.Car
{
    public record CarDto(int Id, string Make, string Model, string Year, string Fuel_type, float Litrage, float Price, float Starting_price);
}
