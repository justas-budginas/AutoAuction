using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.DTOs.Bet
{
    public record BetUpdateDto([Required]float Betting_price);
}
