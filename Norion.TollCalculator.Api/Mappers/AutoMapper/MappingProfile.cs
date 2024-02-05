using AutoMapper;
using Norion.TollCalculator.Api.DTOs;
using Norion.TollCalculator.Domain.Models;

namespace Norion.TollCalculator.Api.Mappers.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PostVehicleDto, Vehicle>().ReverseMap();
        CreateMap<PostVehicleDto, Car>().ReverseMap();
    }
}
