using AutoAukcionas.Data.DTOs.Auth;
using AutoAukcionas.Data.DTOs.Bet;
using AutoAukcionas.Data.DTOs.Car;
using AutoAukcionas.Data.DTOs.Country;
using AutoAukcionas.Data.Entities;
using AutoMapper;

namespace AutoAukcionas.Data
{
    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            CreateMap<Country, CountryDto>();
            CreateMap<CountryPostDto, Country>();
            CreateMap<CountryUpdateDto, Country>();
            CreateMap<Car, CarDto>();
            CreateMap<CarPostDto, Car>();
            CreateMap<UpdateCarDto, Car>();
            CreateMap<Bet, BetDto>();
            CreateMap<BetPostDto, Bet>();
            CreateMap<BetUpdateDto, Bet>();
            CreateMap<AuctionUser, UserDto>();
        }
    }
}
