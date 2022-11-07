using AutoMapper;
using Microservices.RabbitMQ.Enums;
using Microservices.RabbitMQ.Interfaces;
using Microservices.RabbitMQ.Types.Platform;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Core.Interfaces.Http;
using PlatformService.Data.Interfaces;
using PlatformService.Data.Models;
using PlatformService.DTOs;

namespace PlatformService.Web.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusProducer<PlatformEventBase> _messageBusProducer;

        public PlatformsController(
            IMapper mapper,
            IPlatformRepository platformRepository,
            ICommandDataClient commandDataClient,
            IMessageBusProducer<PlatformEventBase> messageBusProducer)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusProducer = messageBusProducer;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platforms = _platformRepository.GetAll().ToArray();
            var result = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);

            return Ok(result);
        }
        
        [HttpGet("{id:int}", Name = nameof(GetById))]
        public ActionResult<PlatformReadDto> GetById(int id)
        {
            var platform = _platformRepository.GetById(id);
            var result = _mapper.Map<PlatformReadDto>(platform);

            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> Create(PlatformCreateDto platformCreate)
        {
            var platformModel = _mapper.Map<Platform>(platformCreate);
            _platformRepository.Create(platformModel);
            _platformRepository.SaveChanges();

            var result = _mapper.Map<PlatformReadDto>(platformModel);

            //Send sync message
            try
            {
                await _commandDataClient.SendPlatformToCommandAsync(result);
            }
            catch (Exception)
            {
                Console.WriteLine("--> Cannot send sync request");
            }

            //Send async message
            try
            {
                var platformPublishedEvent = _mapper.Map<PlatformPublishedEvent>(result);
                platformPublishedEvent.EventType = EventType.PlatformPublished.ToString();
                _messageBusProducer.Publish(platformPublishedEvent);
            }
            catch (Exception)
            {
                Console.WriteLine("--> Cannot send async request");
            }

            return CreatedAtRoute(nameof(GetById), new { result.Id }, result);
        }
    }
}