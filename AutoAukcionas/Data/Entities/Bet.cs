using AutoAukcionas.Auth.Model;
using AutoAukcionas.Data.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.Entities
{
    public class Bet : IUserOwnedResource
    {
        public int ID { get; set; }
        public float Betting_price { get; set; }
        public int CarId { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public Car Car { get; set; }
        public AuctionUser User { get; set; }
    }
}
