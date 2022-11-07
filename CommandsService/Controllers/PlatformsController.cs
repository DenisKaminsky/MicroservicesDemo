using AutoMapper;
using CommandsService.Data.Interfaces;
using CommandsService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/commands/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;

        public PlatformsController(
            IPlatformRepository platformRepository, 
            IMapper mapper)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            Console.WriteLine("--> GET GetAllPlatforms.");

            var platforms = _platformRepository.GetAllPlatforms();
            var result = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);

            return Ok(result);
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Test PlatformsController");
        }
    }
}
