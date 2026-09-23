using System.Security.Claims;
using api.DTOs.Reservation;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public ReservationController(
            IReservationRepository reservationRepo,
            IMapper mapper,
            UserManager<ApiUser> userManager)
        {
            _reservationRepo = reservationRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAll()
        {
            var reservationModels =
                await _reservationRepo.GetAllAsync();

            if (reservationModels == null)
                return NotFound();

            return Ok(
                _mapper.Map<IEnumerable<ReservationDto>>(
                    reservationModels));
        }

        [HttpGet("MyReservations")]
        [Authorize]
        public async Task<IActionResult> GetMyReservations()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var reservations =
                await _reservationRepo.GetUserReservationsAsync(
                    userId);

            return Ok(
                _mapper.Map<IEnumerable<ReservationDto>>(
                    reservations));
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var reservationModel =
                await _reservationRepo.GetUserReservationAsync(
                    id,
                    userId);

            if (reservationModel == null)
                return NotFound(
                    "Reservation not found.");

            return Ok(
                _mapper.Map<ReservationDto>(
                    reservationModel));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(
            [FromBody] CreateReservationRequestDto reservationDto)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(
                    "User ID not found in token.");

            var roomModel =
                await _reservationRepo.GetRoomAsync(
                    reservationDto.RoomId);

            if (roomModel == null)
                return BadRequest(
                    "Room not found.");

            var checkinDate =
                reservationDto.CheckinDate.UtcDateTime;

            var checkOutDate =
                reservationDto.CheckOutDate.UtcDateTime;

            if (checkOutDate <= checkinDate)
            {
                return BadRequest(
                    "End date must be greater than start date.");
            }

            if (checkinDate <= DateTime.UtcNow)
            {
                return BadRequest(
                    "Start date must be greater than current date.");
            }

            var overlappingReservations =
                await _reservationRepo.GetReservationsByRoomId(
                    reservationDto.RoomId,
                    checkinDate,
                    checkOutDate);

            if (overlappingReservations.Any())
            {
                return BadRequest(
                    "Some of the selected dates are already reserved.");
            }

            var reservationModel = new Reservation
            {
                CheckinDate = checkinDate,
                CheckOutDate = checkOutDate,
                ApiUserId = userId,
                PricePerDay = roomModel.BasePricePerDay,
                CreatedAt = DateTime.UtcNow
            };

            var resultModel =
                await _reservationRepo.AddAsync(
                    reservationModel);

            var reservation =
                _mapper.Map<ReservationDto>(
                    resultModel);

            var user =
                await _userManager.GetUserAsync(User);

            if (user != null &&
                await _userManager.IsInRoleAsync(
                    user,
                    "User"))
            {
                await _userManager.RemoveFromRoleAsync(
                    user,
                    "User");

                await _userManager.AddToRoleAsync(
                    user,
                    "Customer");
            }

            return CreatedAtAction(
                nameof(Get),
                new { id = resultModel.Id },
                reservation);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Put(
            int id,
            UpdateReservationRequestDto reservationDto)
        {
            if (id != reservationDto.Id)
                return BadRequest(
                    "Reservation Ids do not match.");

            var reservationModel =
                await _reservationRepo.GetAsync(id);

            if (reservationModel == null)
                return NotFound(
                    "Reservation not found.");

            _mapper.Map(
                reservationDto,
                reservationModel);

            try
            {
                await _reservationRepo.UpdateAsync(
                    reservationModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _reservationRepo.Exists(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _reservationRepo.Exists(id))
                return NotFound(
                    "Reservation not found.");

            await _reservationRepo.DeleteAsync(id);

            return NoContent();
        }
    }
}
