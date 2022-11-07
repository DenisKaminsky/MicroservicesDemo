using AutoMapper;
using CommandsService.Data.Interfaces;
using CommandsService.Data.Models;
using CommandsService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[ApiController]
[Route("api/commands/platforms/{platformId:int}/[controller]")]
public class CommandsController: ControllerBase
{
    private readonly ICommandRepository _commandRepository;
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;
    
    public CommandsController(
        ICommandRepository commandRepository,
        IPlatformRepository platformRepository,
        IMapper mapper)
    {
        _commandRepository = commandRepository;
        _platformRepository = platformRepository;
        _mapper = mapper;
    }

    [HttpGet("")]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsByPlatformId(int platformId)
    {
        Console.WriteLine($"--> GET GetCommandsByPlatformId. PlatformID: {platformId}");

        if (!_platformRepository.PlatformExist(platformId))
        {
            return NotFound();
        }

        var commands = _commandRepository.GetCommandsByPlatformId(platformId);
        var result = _mapper.Map<IEnumerable<CommandReadDto>>(commands);

        return Ok(result);
    }

    [HttpGet("{commandId:int}", Name = nameof(GetCommandByPlatformId))]
    public ActionResult<CommandReadDto> GetCommandByPlatformId(int platformId, int commandId)
    {
        Console.WriteLine($"--> GET GetCommandByPlatformId. PlatformID: {platformId}. CommandID: {commandId}");
        
        var command = _commandRepository.GetCommandByPlatformId(commandId, platformId);
        var result = _mapper.Map<CommandReadDto>(command);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost("")]
    public ActionResult<CommandReadDto> CreateCommand(int platformId, CommandCreateDto commandCreate)
    {
        Console.WriteLine($"--> POST CreateCommand.");

        if (!_platformRepository.PlatformExist(platformId))
        {
            return BadRequest("Platform was not found");
        }

        var command = _mapper.Map<Command>(commandCreate);
        command.PlatformId = platformId;
        _commandRepository.CreateCommand(command);

        var result = _mapper.Map<CommandReadDto>(command);

        return CreatedAtRoute(
            nameof(GetCommandByPlatformId), 
            new { platformId, commandId = command.Id }, 
            result);
    }
}