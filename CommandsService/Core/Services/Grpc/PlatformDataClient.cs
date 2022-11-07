using AutoMapper;
using CommandsService.Core.Interfaces.Grpc;
using CommandsService.Data.Models;
using Grpc.Net.Client;
using Microservices.Grpc;

namespace CommandsService.Core.Services.Grpc;

public class PlatformDataClient: IPlatformDataClient
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public PlatformDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        try
        {
            var grpcPlatform = _configuration["Dependencies:GrpcPlatform"];
            Console.WriteLine($"--> Calling GRPC Service {grpcPlatform}");

            var channel = GrpcChannel.ForAddress(grpcPlatform);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllPlatformsRequest();

            var response = client.GetAllPlatforms(request);

            return _mapper.Map<IEnumerable<Platform>>(response.Platforms);
        }
        catch (Exception ex)
        {
            throw new Exception($"--> Cannot get platforms from GRPC service: {ex}");
        }
    }
}