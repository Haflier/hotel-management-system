using api.DTOs.Drink;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrinkController : ControllerBase
    {
        private readonly IDrinkRepository _drinkRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IMapper _mapper;

        public DrinkController(
            IDrinkRepository drinkRepo,
            IReservationRepository reservationRepo,
            IMapper mapper)
        {
            _drinkRepo = drinkRepo;
            _reservationRepo = reservationRepo;
            _mapper = mapper;
        }

        [HttpGet("ForReservation/{reservationId:int}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> GetForReservation(int reservationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var reservation = await _reservationRepo.GetUserReservationAsync(
                reservationId,
                userId);

            if (reservation == null)
                return NotFound("Reservation not found.");

            var drinks = await _drinkRepo.GetByHotelIdAsync(
                reservation.Room.HotelId);

            return Ok(_mapper.Map<List<DrinkDto>>(drinks));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(
            [FromBody] CreateDrinkRequestDto drinkDto)
        {
            if (drinkDto == null)
                return BadRequest("Drink object is null.");

            var drinkModel = await _drinkRepo.AddAsync(
                _mapper.Map<Drink>(drinkDto));

            var drink = _mapper.Map<DrinkDto>(drinkModel);

            return Created($"/api/Drink/{drinkModel.Id}", drink);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Put(
            int id,
            DrinkDto drinkDto)
        {
            if (id != drinkDto.Id)
                return BadRequest("Drink Ids do not match.");

            var drinkModel = await _drinkRepo.GetAsync(id);

            if (drinkModel == null)
                return NotFound("Drink not found.");

            _mapper.Map(drinkDto, drinkModel);

            try
            {
                await _drinkRepo.UpdateAsync(drinkModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _drinkRepo.Exists(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _drinkRepo.Exists(id))
                return NotFound("Drink not found.");

            await _drinkRepo.DeleteAsync(id);

            return NoContent();
        }
    }
}
