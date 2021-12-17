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
    [Route("/api/bets")]
    public class BetsController : ControllerBase
    {
        private readonly IBetRepository _betRepository;
        private readonly ICarRepository _carRepository;
        private readonly ICountryRepository _CountryRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public BetsController(IBetRepository betRepository, ICarRepository carRepository,
            ICountryRepository CountryRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _betRepository = betRepository;
            _carRepository = carRepository;
            _CountryRepository = CountryRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [Route("/api/bets/{userId}")]
        [HttpGet("{userId}")]
        public async Task<IEnumerable<BetDto>> GetAll(string userId)
        {
            var car = await _betRepository.GetAll(userId);
            return car.Select(o => _mapper.Map<BetDto>(o));
        }
    }
}
