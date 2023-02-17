using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.HTTP;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;

    public PlatformsController(IPlatformRepository platformRepository, IMapper mapper, ICommandDataClient commandDataClient)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var platforms = _platformRepository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }
    
    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        var platform = _platformRepository.GetPlatformById(id);
        if (platform == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<PlatformReadDto>(platform));
    }
    
    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        var platform = _mapper.Map<PlatformCreateDto, Platform>(platformCreateDto);
        _platformRepository.CreatePlatform(platform);
        _platformRepository.SaveChanges();
        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
        
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not send synchronously: {e.Message}");
        }
        
        return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id}, platformReadDto);
    }
}