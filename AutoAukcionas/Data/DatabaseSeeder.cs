using AutoAukcionas.Auth.Model;
using AutoAukcionas.Data.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data
{
    public class DatabaseSeeder
    {
        private readonly UserManager<AuctionUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DatabaseSeeder(UserManager<AuctionUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            foreach (var role in UserRoles.All)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if(!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var newAdmin = new AuctionUser 
            { 
                UserName = "Admin",
                Email = "admin@auction.house",
                Name = "Admin",
                Surname = "Admin"
            };

            var existingAdmin = await _userManager.FindByEmailAsync(newAdmin.Email);
            if(existingAdmin == null)
            {
                var createAdmin = await _userManager.CreateAsync(newAdmin, "Password123!");
                if(createAdmin.Succeeded)
                {
                    await _userManager.AddToRolesAsync(newAdmin, UserRoles.All);
                }
            }    
        }
    }
}
