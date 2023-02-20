using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/c/platforms/{platformId}/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet(Name = "GetCommandsForPlatform")]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"--> Getting Commands for Platform: {platformId}");
        if (!_repository.PlatformExists(platformId))
        {
            return NotFound();
        }

        var commands = _repository.GetCommandsForPlatform(platformId);
        return Ok(_mapper.Map<CommandReadDto>(commands));
    }
    
    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Getting Command: {commandId} for Platform: {platformId}");
        if (!_repository.PlatformExists(platformId))
        {
            return NotFound();
        }

        var command = _repository.GetCommand(platformId, commandId);
        if (command == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CommandReadDto>(command));
    }
    
    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
    {
        Console.WriteLine($"--> Creating Command for Platform: {platformId}");
        if (!_repository.PlatformExists(platformId))
        {
            return NotFound();
        }

        var command = _mapper.Map<Command>(commandCreateDto);
        _repository.CreateCommand(platformId, command);
        _repository.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);
        return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId, commandId = commandReadDto.Id}, commandReadDto);
    }
}