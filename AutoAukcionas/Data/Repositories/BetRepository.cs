using AutoAukcionas.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.Repositories
{
    public interface IBetRepository
    {
        Task<List<Bet>> GetAll(int CountryId, int carId);
        Task<List<Bet>> GetAll(string UserId);
        Task<Bet> Get(int CountryId, int carId, int betId);
        Task Create(Bet car);
        Task Put(Bet car);
        Task Delete(Bet car);
    }

    public class BetRepository : IBetRepository
    {
        private readonly AuctionContext _auctionContext;
        public BetRepository(AuctionContext auctionContext)
        {
            _auctionContext = auctionContext;
        }

        public async Task<List<Bet>> GetAll(int CountryId, int carId)
        {
            return await _auctionContext.Bet.Where(o => o.Car.CountryId == CountryId && o.CarId == carId).ToListAsync();
        }

        public async Task<List<Bet>> GetAll(string UserId)
        {
            return await _auctionContext.Bet.Where(o => o.UserId == UserId).ToListAsync();
        }

        public async Task<Bet> Get(int CountryId, int carId, int betId)
        {
            return await _auctionContext.Bet.FirstOrDefaultAsync(o => o.Car.CountryId == CountryId && o.CarId == carId && o.ID == betId);
        }

        public async Task Create(Bet bet)
        {
            _auctionContext.Bet.Add(bet);
            await _auctionContext.SaveChangesAsync();
        }

        public async Task Put(Bet bet)
        {
            _auctionContext.Bet.Update(bet);
            await _auctionContext.SaveChangesAsync();
        }

        public async Task Delete(Bet bet)
        {
            _auctionContext.Bet.Remove(bet);
            await _auctionContext.SaveChangesAsync();
        }
    }
}
