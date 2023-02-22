using CommandService.Models;

namespace CommandService.SyncDataServices.GRPC;

public interface IPlatformDataClient
{
    IEnumerable<Platform> ReturnAllPlatforms();
}