using AutoMapper;
using CommandsService.Data.Models;
using Microservices.RabbitMQ.Types.Platform;

namespace CommandsService.DTOs.MapperProfiles;

public class CommandsServiceProfile: Profile
{
    public CommandsServiceProfile()
    {
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();

        CreateMap<Platform, PlatformReadDto>();

        CreateMap<PlatformPublishedEvent, Platform>()
            .ForMember(x => x.ExternalId, opt => opt.MapFrom(src => src.Id));
    }
}