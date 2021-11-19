using AutoAukcionas.Auth.Model;
using AutoAukcionas.Data.DTOs.Car;
using AutoAukcionas.Data.Entities;
using AutoAukcionas.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Controllers
{
    [ApiController]
    [Route("/api/Country/{CountryId}/car")]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly ICountryRepository _CountryRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public CarController(ICarRepository carRepository, ICountryRepository CountryRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _carRepository = carRepository;
            _CountryRepository = CountryRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IEnumerable<CarDto>> GetAll(int CountryId)
        {
            var car = await _carRepository.GetAll(CountryId);
            return car.Select(o => _mapper.Map<CarDto>(o));
        }

        [HttpGet("{carId}")]
        public async Task<ActionResult<CarDto>> Get(int CountryId, int carId)
        {
            var car = await _carRepository.Get(CountryId, carId);
            if (car == null) return NotFound("This car does not exist!");

            return Ok(_mapper.Map<CarDto>(car));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Seller)]
        public async Task<ActionResult<CarDto>> Create(int CountryId, CarPostDto dto)
        {
            var Country = await _CountryRepository.Get(CountryId);
            if (Country == null) return NotFound("This Country does not exist!");

            var car = _mapper.Map<Car>(dto);
            car.CountryId = CountryId;
            car.UserId = User.Claims.Where(c => c.Type == "userId").Single().Value;

            await _carRepository.Create(car);
            return Created("/api/Country/{CountryId}/car/{car.ID}", _mapper.Map<CarDto>(car));

        }

        [HttpPut("{carId}")]
        [Authorize(Roles = UserRoles.Seller)]
        public async Task<ActionResult<CarDto>> Edit(int CountryId, int carId, UpdateCarDto dto)
        {
            var Country = await _CountryRepository.Get(CountryId);
            if (Country == null) return NotFound("This Country does not exist!");

            var oldCar = await _carRepository.Get(CountryId, carId);
            if (oldCar == null) return NotFound("This car does not exist!");

            var authResult = await _authorizationService.AuthorizeAsync(User, oldCar, "SameUser");
            if (!authResult.Succeeded) return Forbid();

            _mapper.Map(dto, oldCar);
            
            await _carRepository.Put(oldCar);
            return Ok(_mapper.Map<CarDto>(oldCar));

        }

        [HttpDelete("{carId}")]
        [Authorize(Roles = UserRoles.Seller + ","+ UserRoles.Admin)]
        public async Task<ActionResult> Delete(int CountryId, int carId)
        {
            var car = await _carRepository.Get(CountryId, carId);
            if (car == null) return NotFound("This car does not exist!");

            var authResult = await _authorizationService.AuthorizeAsync(User, car, "SameUser");
            if (!authResult.Succeeded) return Forbid();

            await _carRepository.Delete(car);
            return NoContent();
        }
    }
}
