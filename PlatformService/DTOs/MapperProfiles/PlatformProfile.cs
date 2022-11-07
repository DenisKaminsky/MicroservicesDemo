using AutoMapper;
using Microservices.Grpc;
using Microservices.RabbitMQ.Types.Platform;
using PlatformService.Data.Models;

namespace PlatformService.DTOs.MapperProfiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        //Model -> DTO
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(x => x.PlatformId, opt => opt.MapFrom(src => src.Id));

        //DTO -> Model
        CreateMap<PlatformCreateDto, Platform>();

        //DTO -> DTO
        CreateMap<PlatformReadDto, PlatformPublishedEvent>();
    }
}