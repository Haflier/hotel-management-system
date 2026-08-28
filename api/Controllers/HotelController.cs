using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Hotel;
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
    public class HotelController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepo;
        private readonly IMapper _mapper;
        public HotelController(IHotelRepository hotelRepo, IMapper mapper)
        {
            _hotelRepo = hotelRepo;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var hotelModels = await _hotelRepo.GetAllAsync();

            if (hotelModels == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<HotelDto>>(hotelModels));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var hotelModel = await _hotelRepo.GetAsync(id);

            if (hotelModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<HotelDto>(hotelModel));
        }

        [HttpGet("AllDetails/{id:int}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var hotelModel = await _hotelRepo.GetDetails(id);

            if (hotelModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<HotelDetailDto>(hotelModel));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateHotelRequestDto hotelDto)
        {
            if (hotelDto == null) return BadRequest("Hotel object is null");
            var HotelModel = await _hotelRepo.AddAsync(_mapper.Map<Hotel>(hotelDto));
            return CreatedAtAction(nameof(Get), new { id = HotelModel.Id }, HotelModel);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutHotel(int id, UpdateHotelRequestDto hotelDto)
        {
            if (id != hotelDto.Id) return BadRequest("Hotel Ids do not match");

            var hotelModel = await _hotelRepo.GetAsync(id);
            if (hotelModel == null) return BadRequest("Hotel not found");

            _mapper.Map(hotelDto, hotelModel);

            try
            {
                await _hotelRepo.UpdateAsync(hotelModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _hotelRepo.Exists(id))
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
            if (!await _hotelRepo.Exists(id))
            {
                return NotFound("Hotel not found.");
            }

            await _hotelRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
