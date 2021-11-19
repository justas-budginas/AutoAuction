using AutoAukcionas.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAll();
        Task<Country> Get(int id);
        Task Create(Country Country);
        Task Put(Country Country);
        Task Delete(Country Country);
        Task<bool> Exist(string name);
    }

    public class CountryRepository : ICountryRepository
    {
        private readonly AuctionContext _auctionContext;
        public CountryRepository(AuctionContext auctionContext)
        {
            _auctionContext = auctionContext;
        }
        public async Task<IEnumerable<Country>> GetAll()
        {
            return await _auctionContext.Country.ToListAsync();
        }

        public async Task<Country> Get(int id)
        {
            return await _auctionContext.Country.FirstOrDefaultAsync(o => o.ID == id);
        }

        public async Task Create(Country Country)
        {
            _auctionContext.Country.Add(Country);
            await _auctionContext.SaveChangesAsync();
        }

        public async Task Put(Country Country)
        {
            _auctionContext.Country.Update(Country);
            await _auctionContext.SaveChangesAsync();
        }

        public async Task Delete(Country Country)
        {
            _auctionContext.Country.Remove(Country);
            await _auctionContext.SaveChangesAsync();
        }

        public async Task<bool> Exist(string name)
        {
            bool exist = false;

            var user = await _auctionContext.Country.FirstOrDefaultAsync(o => o.Name == name);

            if (user != null)
            {
                exist = true;
            }

            return exist;
        }
    }
}
