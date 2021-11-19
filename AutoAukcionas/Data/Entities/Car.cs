using AutoAukcionas.Auth.Model;
using AutoAukcionas.Data.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        [Required]
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public AuctionUser User {get; set;}
    }
}
