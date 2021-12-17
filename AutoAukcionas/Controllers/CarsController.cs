using AutoAukcionas.Auth.Model;
using AutoAukcionas.Data.DTOs.Car;
using AutoAukcionas.Data.Entities;
using AutoAukcionas.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Controllers
{
    [Route("/api/cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly ICountryRepository _CountryRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public CarsController(ICarRepository carRepository, ICountryRepository CountryRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _carRepository = carRepository;
            _CountryRepository = CountryRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [Route("/api/cars/{userId}")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetAllUser(string userId)
        {
            var car = await _carRepository.GetAll(userId);
            return Ok(car.Select(o => _mapper.Map<CarDto>(o)));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetAllCars()
        {
            var car = await _carRepository.GetAllCars();
            return Ok(car.Select(o => _mapper.Map<CarDto>(o)));
        }

        [Route("/api/cars/{countryId}/{carId}")]
        [HttpDelete("{carId}")]
        public async Task<ActionResult> Delete(int CountryId, int carId)
        {
            var car = await _carRepository.Get(CountryId, carId);
            if (car == null) return NotFound("This car does not exist!");

            await _carRepository.Delete(car);
            return NoContent();
        }
    }
}
