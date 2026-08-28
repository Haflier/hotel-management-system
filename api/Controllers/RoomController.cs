using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Room;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _roomRepo;
        private readonly IMapper _mapper;
        public RoomController(IRoomRepository roomRepo, IMapper mapper)
        {
            _roomRepo = roomRepo;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var roomModels = await _roomRepo.GetAllAsync();

            if (roomModels == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<RoomDto>>(roomModels));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var roomModel = await _roomRepo.GetAsync(id);

            if (roomModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RoomDto>(roomModel));
        }

        [HttpGet("AllDetails/{id:int}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var roomModel = await _roomRepo.GetDetails(id);

            if (roomModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RoomDetailDto>(roomModel));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateRoomRequestDto roomDto)
        {
            var roomModel = await _roomRepo.AddAsync(_mapper.Map<Room>(roomDto));
            var room = _mapper.Map<RoomDto>(roomModel);
            return CreatedAtAction(nameof(GetDetails), new { id = roomModel.Id }, room);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutRoom(int id, RoomDto roomDto)
        {
            if (id != roomDto.Id) return BadRequest("Room Ids do not match");

            var roomModel = await _roomRepo.GetAsync(id);
            if (roomModel == null) return BadRequest("Room not found");

            _mapper.Map(roomDto, roomModel);

            try
            {
                await _roomRepo.UpdateAsync(roomModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _roomRepo.Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var roomModel = await _roomRepo.Delete(id);

            if (roomModel == null) return NotFound("Room not found");

            return NoContent();
        }

        [HttpPost("{roomId}/services/{serviceId}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> AddServiceToRoom(int roomId, int serviceId)
        {
            var roomService = await _roomRepo.AddServiceToRoomAsync(roomId, serviceId);

            if (roomService == null)
            {
                return NotFound("Room or service not found.");
            }

            return Ok(_mapper.Map<RoomServiceDto>(roomService));
        }

        [HttpDelete("{roomId}/services/{serviceId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveServiceFromRoom(int roomId, int serviceId)
        {
            var roomService = await _roomRepo.RemoveServiceFromRoomAsync(roomId, serviceId);

            if (roomService == null)
            {
                return NotFound("Room or service not found.");
            }

            return NoContent();
        }
    }
}
