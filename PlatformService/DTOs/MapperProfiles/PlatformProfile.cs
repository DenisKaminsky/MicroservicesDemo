using AutoMapper;
using Microservices.RabbitMQ.Types.Platform;
using PlatformService.Models;

namespace PlatformService.DTOs.MapperProfiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        //Model -> DTO
        CreateMap<Platform, PlatformReadDto>();

        //DTO -> Model
        CreateMap<PlatformCreateDto, Platform>();

        //DTO -> DTO
        CreateMap<PlatformReadDto, PlatformPublishedEvent>();
    }
}