using AutoAukcionas.Auth.Model;
using AutoAukcionas.Data.DTOs.Bet;
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
    [Route("/api/Country/{CountryId}/car/{CarId}/bet")]
    public class BetController : ControllerBase
    {
        private readonly IBetRepository _betRepository;
        private readonly ICarRepository _carRepository;
        private readonly ICountryRepository _CountryRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public BetController(IBetRepository betRepository ,ICarRepository carRepository, 
            ICountryRepository CountryRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _betRepository = betRepository;
            _carRepository = carRepository;
            _CountryRepository = CountryRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IEnumerable<BetDto>> GetAll(int CountryId, int CarId)
        {
            var car = await _betRepository.GetAll(CountryId, CarId);
            return car.Select(o => _mapper.Map<BetDto>(o));
        }

        [HttpGet("{betId}")]
        public async Task<ActionResult<BetDto>> Get(int CountryId, int carId, int betId)
        {
            var bet = await _betRepository.Get(CountryId, carId, betId);
            if (bet == null) return NotFound("This bet does not exist!");

            return Ok(_mapper.Map<BetDto>(bet));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Buyer)]
        public async Task<ActionResult<BetDto>> Create(int CountryId, int CarId, BetPostDto dto)
        {
            var car = await _carRepository.Get(CountryId, CarId);
            if (car == null) return NotFound("This Car does not exist!");

            var bet = _mapper.Map<Bet>(dto);
            bet.CarId = CarId;
            bet.CountryId = CountryId;
            bet.UserId = User.Claims.Where(c => c.Type == "userId").Single().Value;

            await _betRepository.Create(bet);
            car.Price = bet.Betting_price;
            await _carRepository.Put(car);
            return Created("/api/Country/{CountryId}/car/{CarId}/bet/{bet.ID}", _mapper.Map<BetDto>(bet));

        }

        [HttpPut("{betId}")]
        [Authorize(Roles = UserRoles.Buyer)]
        public async Task<ActionResult<BetDto>> Edit(int CountryId, int CarId, int betId, BetUpdateDto dto)
        {
            var Country = await _CountryRepository.Get(CountryId);
            if (Country == null) return NotFound("This Country does not exist!");

            var oldCar = await _carRepository.Get(CountryId, CarId);
            if (oldCar == null) return NotFound("This car does not exist!");

            var oldBet = await _betRepository.Get(CountryId, CarId, betId);
            if (oldBet == null) return NotFound("This bet does not exist!");

            var authResult = await _authorizationService.AuthorizeAsync(User, oldBet, "SameUser");
            if (!authResult.Succeeded) return Forbid();

            _mapper.Map(dto, oldBet);

            await _betRepository.Put(oldBet);
            return Ok(_mapper.Map<BetDto>(oldBet));

        }

        [HttpDelete("{betId}")]
        [Authorize(Roles = UserRoles.Buyer+ "," + UserRoles.Admin)]
        public async Task<ActionResult> Delete(int CountryId, int CarId, int betId)
        {
            var bet = await _betRepository.Get(CountryId, CarId, betId);
            if (bet == null) return NotFound("This bet does not exist!");

            var authResult = await _authorizationService.AuthorizeAsync(User, bet, "SameUser");
            if (!authResult.Succeeded) return Forbid();

            await _betRepository.Delete(bet);

            var bets = await _betRepository.GetAll(CountryId, CarId);

            Bet lastbet;

            if(bets.Count > 0)
            {
                lastbet = bets.Last();
            }
            else
            {
                lastbet = null;
            }

            var car = await _carRepository.Get(CountryId, CarId);

            if(lastbet == null)
            {
                car.Price = car.Starting_Price;
            }
            else
            {
                car.Price = lastbet.Betting_price;
            }

            

            await _carRepository.Put(car);

            return NoContent();
        }
    }
}
