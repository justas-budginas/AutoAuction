using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoAukcionas.Data.DTOs.Auth;
using AutoAukcionas.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoAukcionas.Data
{
    public class AuctionContext : IdentityDbContext<AuctionUser>
    {
        public DbSet<Country> Country { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<Bet> Bet { get; set; }
        public DbSet<RefreshToken> RefreshToken {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=AutoAuction");
            //optionsBuilder.UseSqlServer("Server=tcp:autoaukcionasdbserver.database.windows.net,1433;Initial Catalog=AutoAukcionas_db;Persist Security Info=False;User ID=justas;Password=SuperDuperSeceretPassword123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("MyDbConnection"));
        }
    }
}
