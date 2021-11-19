using AutoAukcionas.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<string> Get(string Username);
        Task Create(RefreshToken token);
        Task Update(RefreshToken token);
        Task<bool> Exist(string Username);
    }
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuctionContext _auctionContext;

        public RefreshTokenRepository(AuctionContext auctionContext)
        {
            _auctionContext = auctionContext;
        }

        public async Task<string> Get(string Username)
        {
            var token = await _auctionContext.RefreshToken.FirstOrDefaultAsync(o => o.Username == Username);
            return token.Refreshtoken;
        }

        public async Task Create(RefreshToken token)
        {
            _auctionContext.RefreshToken.Add(token);
            await _auctionContext.SaveChangesAsync();
        }

        public async Task Update(RefreshToken token) 
        {
            _auctionContext.RefreshToken.Update(token);
            await _auctionContext.SaveChangesAsync();
        }

        public async Task<bool> Exist(string Username)
        {
            bool exist = false;

            var user = await _auctionContext.RefreshToken.FirstOrDefaultAsync(o => o.Username == Username);
            
            if(user != null)
            {
                exist = true;
            }

            return exist;
        }
    }
}
