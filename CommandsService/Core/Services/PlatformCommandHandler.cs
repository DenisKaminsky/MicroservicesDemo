using AutoMapper;
using CommandsService.Data.Interfaces;
using CommandsService.Data.Models;
using MediatR;
using Microservices.RabbitMQ.Types.Platform;

namespace CommandsService.Core.Services
{
    public class PlatformCommandHandler : IRequestHandler<PlatformPublishedEvent>
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;

        public PlatformCommandHandler(IMapper mapper, IPlatformRepository platformRepository)
        {
            _mapper = mapper;
            _platformRepository = platformRepository;
        }

        public Task<Unit> Handle(PlatformPublishedEvent request, CancellationToken cancellationToken)
        {
            try
            {
                var platform = _mapper.Map<Platform>(request);
                if (!_platformRepository.ExternalPlatformExist(platform.ExternalId))
                {
                    _platformRepository.CreatePlatform(platform);
                }
                else
                {
                    Console.WriteLine("--> ExternalId already exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Cannot add platform: {ex}");
            }

            return Task.FromResult(Unit.Value);
        }
    }
}
