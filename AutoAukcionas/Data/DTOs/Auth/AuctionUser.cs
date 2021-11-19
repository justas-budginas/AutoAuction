using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.DTOs.Auth
{
    public class AuctionUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        public string Surname { get; set; }

    }
}
