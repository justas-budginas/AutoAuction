using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Auth.Model
{
    public class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string Buyer = nameof(Buyer);
        public const string Seller = nameof(Seller);

        public static readonly IReadOnlyCollection<string> All = new[] { Admin, Buyer, Seller };
    }
}
