using api.DTOs.Food;
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
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepository _foodRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IMapper _mapper;

        public FoodController(
            IFoodRepository foodRepo,
            IReservationRepository reservationRepo,
            IMapper mapper)
        {
            _foodRepo = foodRepo;
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

            var foods = await _foodRepo.GetByHotelIdAsync(
                reservation.Room.HotelId);

            return Ok(_mapper.Map<List<FoodDto>>(foods));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(
            [FromBody] CreateFoodRequestDto foodDto)
        {
            if (foodDto == null)
                return BadRequest("Food object is null.");

            var foodModel = await _foodRepo.AddAsync(
                _mapper.Map<Food>(foodDto));

            var food = _mapper.Map<FoodDto>(foodModel);

            return Created($"/api/Food/{foodModel.Id}", food);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Put(
            int id,
            FoodDto foodDto)
        {
            if (id != foodDto.Id)
                return BadRequest("Food Ids do not match.");

            var foodModel = await _foodRepo.GetAsync(id);

            if (foodModel == null)
                return NotFound("Food not found.");

            _mapper.Map(foodDto, foodModel);

            try
            {
                await _foodRepo.UpdateAsync(foodModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _foodRepo.Exists(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _foodRepo.Exists(id))
                return NotFound("Food not found.");

            await _foodRepo.DeleteAsync(id);

            return NoContent();
        }
    }
}
