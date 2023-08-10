using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _respository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        // inject the repository and mapper: Called Constructor Injection Repository Pattern
        public PlatformsController(
            IPlatformRepo repository,
            IMapper mapper,
            ICommandDataClient commandDataClient
            )
        {
            _respository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms...");

            var platformItems = _respository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine("--> Getting Platform by Id...");

            var platformItem = _respository.GetPlatformById(id); // returns a Platform object i.e an entity

            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem)); // returns a PlatformReadDto object i.e a dto
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            // check if the platform already exists by name
            var platformModelExist = _respository.GetPlatformByName(platformCreateDto.Name);

            if (platformModelExist != null)
            {
                return BadRequest("Platform already exists");
            }


            Console.WriteLine("--> Creating Platform...");
            //                             source -> target
            var platformModel = _mapper.Map<Platform>(platformCreateDto); // here we are mapping a dto to an entity

            _respository.CreatePlatform(platformModel);
            _respository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel); // here we are mapping an entity to a DTO

            // send the platformReadDto to the CommandService
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            } catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id}, platformReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePlatform(int id, PlatformUpdateDto platformUpdateDto)
        {
            Console.WriteLine("--> Updating Platform...");

            var platformModelFromRepo = _respository.GetPlatformById(id);

            if (platformModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(platformUpdateDto, platformModelFromRepo); // here we are mapping a dto to an entity

            _respository.UpdatePlatform(platformModelFromRepo);

            _respository.SaveChanges();

            // return NoContent();
            // return Ok("Update Successful");
            return Ok(platformModelFromRepo);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePlatform(int id)
        {
            Console.WriteLine("--> Deleting Platform...");

            var platformModelFromRepo = _respository.GetPlatformById(id);

            if (platformModelFromRepo == null)
            {
                return NotFound();
            }

            _respository.DeletePlatform(platformModelFromRepo);

            _respository.SaveChanges();

            return NoContent();
        }

    }
}