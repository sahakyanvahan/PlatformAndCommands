using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.GRPC;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepository platformRepository, IMapper mapper)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
    }
    
    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var response = new PlatformResponse();
        var platforms = _platformRepository.GetAllPlatforms();
        
        response.Platforms.AddRange(_mapper.Map<IEnumerable<GrpcPlatformModel>>(platforms));
        return Task.FromResult(response);
    }
}