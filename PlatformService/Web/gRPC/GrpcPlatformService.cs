using AutoMapper;
using Grpc.Core;
using Microservices.Grpc;
using PlatformService.Data.Interfaces;

namespace PlatformService.Web.gRPC;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepository platformRepository, IMapper mapper)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
    }

    public override Task<GetAllPlatformsResponse> GetAllPlatforms(GetAllPlatformsRequest request, ServerCallContext context)
    {
        var platforms = _platformRepository.GetAll().ToArray();

        var response = new GetAllPlatformsResponse();
        response.Platforms.AddRange(_mapper.Map<IEnumerable<GrpcPlatformModel>>(platforms));

        return Task.FromResult(response);
    }
}