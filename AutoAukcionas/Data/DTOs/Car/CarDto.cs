using Microsoft.AspNetCore.Http;

namespace AutoAukcionas.Data.DTOs.Car
{
    public record CarDto(int Id, string Make, string Model, string Year, string Fuel_type, float Litrage, float Price, float Starting_price, string CarImage, string CountryId);
}
