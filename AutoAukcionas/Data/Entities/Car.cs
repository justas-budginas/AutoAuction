using AutoAukcionas.Auth.Model;
using AutoAukcionas.Data.DTOs.Auth;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AutoAukcionas.Data.Entities
{
    public class Car : IUserOwnedResource
    {
        public int ID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Fuel_type { get; set; }
        public float Litrage { get; set; }
        public float Price { get; set; }
        public float Starting_Price { get; set; }
        public string CarImage { get; set; }
        [Required]
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public AuctionUser User {get; set;}
    }
}
