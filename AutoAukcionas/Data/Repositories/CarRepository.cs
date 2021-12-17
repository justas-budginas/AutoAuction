using AutoAukcionas.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Data.Repositories
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAll(int CountryId);
        Task<List<Car>> GetAll(string UserId);
        Task<List<Car>> GetAllCars();
        Task<Car> Get(int CountryId, int carId);
        Task Create(Car car);
        Task Put(Car car);
        Task Delete(Car car);
    }

    public class CarRepository : ICarRepository
    {
        private readonly AuctionContext _auctionContext;
        public CarRepository(AuctionContext auctionContext)
        {
            _auctionContext = auctionContext;
        }

        public async Task<List<Car>> GetAll(int CountryId)
        {
            return await _auctionContext.Car.Where(o => o.Country.ID == CountryId).ToListAsync();
        }

        public async Task<List<Car>> GetAll(string UserId)
        {
            return await _auctionContext.Car.Where(o=> o.UserId == UserId).ToListAsync();
        }

        public async Task<List<Car>> GetAllCars()
        {
            return await _auctionContext.Car.ToListAsync();
        }

        public async Task<Car> Get(int CountryId, int carId)
        {
            return await _auctionContext.Car.FirstOrDefaultAsync(o => o.Country.ID == CountryId && o.ID == carId);
        }

        public async Task Create(Car car)
        {
            _auctionContext.Car.Add(car);
            await _auctionContext.SaveChangesAsync();
        }

        public async Task Put(Car car)
        {
            _auctionContext.Car.Update(car);
            await _auctionContext.SaveChangesAsync();
        }

        public async Task Delete(Car car)
        {
            _auctionContext.Car.Remove(car);
            await _auctionContext.SaveChangesAsync();
        }
    }
}
