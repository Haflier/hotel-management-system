using System.Security.Claims;
using api.DTOs.OrderItem;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepo;
        private readonly IFoodRepository _foodRepo;
        private readonly IDrinkRepository _drinkRepo;
        private readonly IReservationRepository _reservationRepo;
        public OrderItemController(IOrderItemRepository orderItemRepo, IMapper mapper
            , IOrderRepository orderRepo, IFoodRepository foodRepo, IDrinkRepository drinkRepo, IReservationRepository reservationRep)
        {
            _orderItemRepo = orderItemRepo;
            _mapper = mapper;
            _orderRepo = orderRepo;
            _foodRepo = foodRepo;
            _drinkRepo = drinkRepo;
            _reservationRepo = reservationRep;
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var orderItem = await _orderItemRepo.GetUserOrderItemAsync(
                id,
                userId);

            if (orderItem == null)
                return NotFound("OrderItem not found.");

            return Ok(_mapper.Map<OrderItemDto>(orderItem));
        }

        [HttpPost]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> Create(
            [FromBody] CreateOrderItemRequestDto orderItemDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (orderItemDto == null)
                return BadRequest("OrderItem object is null.");

            if (orderItemDto.Quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            if (orderItemDto.FoodId.HasValue == orderItemDto.DrinkId.HasValue)
                return BadRequest("OrderItem must contain either a food or a drink.");

            var reservation = await _reservationRepo.GetUserReservationAsync(
                orderItemDto.ReservationId,
                userId);

            if (reservation == null)
                return NotFound("Reservation not found.");

            var hotelId = reservation.Room.HotelId;

            Food? foodModel = null;
            Drink? drinkModel = null;

            if (orderItemDto.FoodId.HasValue)
            {
                foodModel = await _foodRepo.GetAsync(orderItemDto.FoodId.Value);

                if (foodModel == null)
                    return NotFound("Food not found.");

                if (foodModel.HotelId != hotelId)
                    return BadRequest("Food does not belong to the reserved hotel.");
            }
            else
            {
                drinkModel = await _drinkRepo.GetAsync(orderItemDto.DrinkId!.Value);

                if (drinkModel == null)
                    return NotFound("Drink not found.");

                if (drinkModel.HotelId != hotelId)
                    return BadRequest("Drink does not belong to the reserved hotel.");
            }

            var existingOrder = await _orderRepo.CheckExistingOrder(userId);

            if (existingOrder != null &&
                existingOrder.ReservationId != reservation.Id)
            {
                return BadRequest(
                    "You already have an active order for another reservation.");
            }

            if (existingOrder == null)
            {
                existingOrder = new Order
                {
                    CreatedAt = DateTime.Now,
                    ApiUserId = userId,
                    ReservationId = reservation.Id,
                    IsFinalized = false,
                    Items = new List<OrderItem>()
                };

                await _orderRepo.AddAsync(existingOrder);
            }

            var orderItem = new OrderItem
            {
                UnitPrice = foodModel?.Price ?? drinkModel!.Price,
                Quantity = orderItemDto.Quantity,
                FoodId = foodModel?.Id,
                DrinkId = drinkModel?.Id,
                OrderId = existingOrder.Id
            };

            await _orderItemRepo.AddAsync(orderItem);

            existingOrder.TotalPrice += orderItem.TotalPrice;

            await _orderRepo.UpdateAsync(existingOrder);

            return Ok(_mapper.Map<OrderItemDto>(orderItem));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Put(int id, UpdateOrderItemRequestDto orderItemDto)
        {
            if (id != orderItemDto.Id) return BadRequest("OrderItem Ids do not match");

            var orderItemModel = await _orderItemRepo.GetAsync(id);
            if (orderItemModel == null) return BadRequest("OrderItem not found");

            _mapper.Map(orderItemDto, orderItemModel);

            try
            {
                await _orderItemRepo.UpdateAsync(orderItemModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _orderItemRepo.Exists(id))
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
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _orderItemRepo.Exists(id))
            {
                return NotFound("OrderItem not found.");
            }

            await _orderItemRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
