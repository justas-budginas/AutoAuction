using System.ComponentModel.DataAnnotations;

namespace AutoAukcionas.Data.DTOs.Country
{
    public record CountryUpdateDto([Required] string Name);
}
