using System.Security.Claims;
using api.DTOs.OrderItem;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
        public OrderItemController(IOrderItemRepository orderItemRepo, IMapper mapper
            , IOrderRepository orderRepo, IFoodRepository foodRepo, IDrinkRepository drinkRepo)
        {
            _orderItemRepo = orderItemRepo;
            _mapper = mapper;
            _orderRepo = orderRepo;
            _foodRepo = foodRepo;
            _drinkRepo = drinkRepo;
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> Get(int id)
        {
            var orderItem = await _orderItemRepo.GetAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OrderItemDto>(orderItem));
        }

        [HttpPost]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> Create([FromBody] CreateOrderItemRequestDto orderItemDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (orderItemDto == null) return BadRequest("OrderItem object is null");

            if (orderItemDto.FoodId != null && orderItemDto.DrinkId != null)
                return BadRequest("OrderItem must have either a food or a drink in each request");

            if (orderItemDto != null && !(orderItemDto.FoodId == null && orderItemDto.DrinkId == null)
                && !(orderItemDto.FoodId != null && orderItemDto.DrinkId != null))
            {
                var existingOrder = await _orderRepo.CheckExistingOrder(userId);
                if (existingOrder == null)
                {
                    existingOrder = new Order
                    {
                        CreatedAt = DateTime.Now,
                        ApiUserId = userId,
                        IsFinalized = false,
                        Items = new List<OrderItem>()
                    };

                    await _orderRepo.AddAsync(existingOrder);
                }

                var foodModel = await _foodRepo.GetAsync(orderItemDto.FoodId);
                var drinkModel = await _drinkRepo.GetAsync(orderItemDto.DrinkId);

                var orderItem = new OrderItem
                {
                    UnitPrice = foodModel?.Price ?? drinkModel?.Price
                        ?? throw new InvalidOperationException("Both Food and Drink prices are null."),
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

            return StatusCode(500);
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
