using AutoAukcionas.Auth.Model;
using AutoAukcionas.Data.DTOs.Country;
using AutoAukcionas.Data.Entities;
using AutoAukcionas.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAukcionas.Controllers
{
    [ApiController]
    [Route("api/country")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _CountryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository CountryRepository, IMapper mapper)
        {
            _CountryRepository = CountryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CountryDto>> GetAll()
        {
            var car = await _CountryRepository.GetAll();
            return car.Select(o => _mapper.Map<CountryDto>(o));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> Get(int id)
        {
            var Country = await _CountryRepository.Get(id);
            if (Country == null) return NotFound("This Country does not exist!");

            return Ok(_mapper.Map<CountryDto>(Country));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<CountryDto>> Create(CountryPostDto dto)
        {
            var exist = await _CountryRepository.Exist(dto.Name);
            if (exist) return Conflict(dto.Name + " already exists!");

            var Country = _mapper.Map<Country>(dto);

            await _CountryRepository.Create(Country);

            // 201
            // Created Country
            return Created($"/api/country/'{Country.ID}'", _mapper.Map<CountryDto>(Country));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<CountryDto>> Edit(int id, CountryUpdateDto dto)
        {
            var Country = await _CountryRepository.Get(id);
            if (Country == null) return NotFound("This Country does not exist!");

            _mapper.Map(dto, Country);

            await _CountryRepository.Put(Country);

            return Ok(_mapper.Map<CountryDto>(Country));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<CountryDto>> Delete(int id)
        {
            var Country = await _CountryRepository.Get(id);
            if (Country == null) return NotFound("This Country does not exist!");

            await _CountryRepository.Delete(Country);

            return NoContent();
        }
    }
}
