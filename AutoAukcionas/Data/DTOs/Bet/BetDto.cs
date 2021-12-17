using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.DTOs.Bet
{
    public record BetDto(int ID, float Betting_price, string userid, string countryid, string carid);
}
