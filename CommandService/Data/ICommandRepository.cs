using CommandService.Models;

namespace CommandService.Data;

public interface ICommandRepository
{
    bool SaveChanges();
    
    IEnumerable<Platform> GetAllPlatforms();
    
    void CreatePlatform(Platform platform);
    
    bool PlatformExists(int platformId);
    
    void CreateCommand(int platformId, Command command);
    
    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    
    Command GetCommand(int platformId, int commandId);
}