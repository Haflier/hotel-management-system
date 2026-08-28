using api.DTOs.Order;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        public OrderController(IOrderRepository orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAll()
        {
            var orderModels = await _orderRepo.GetAllDetails();

            if (orderModels == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<OrderDto>>(orderModels));
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> Get(int id)
        {
            var orderModel = await _orderRepo.GetDetail(id);

            if (orderModel == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(_mapper.Map<OrderDto>(orderModel));
        }

        [HttpPost("{orderId}/finalize")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> FinalizeOrder(int orderId)
        {
            var orderModel = await _orderRepo.GetDetail(orderId);

            if (orderModel == null)
            {
                return NotFound("Order not found.");
            }

            if (orderModel.IsFinalized)
            {
                return BadRequest("Order is already finalized.");
            }

            orderModel.IsFinalized = true;
            await _orderRepo.UpdateAsync(orderModel);
            return Ok(_mapper.Map<OrderDto>(orderModel));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _orderRepo.Exists(id))
            {
                return NotFound("Order not found.");
            }

            await _orderRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
