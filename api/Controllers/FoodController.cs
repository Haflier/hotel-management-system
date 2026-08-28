using api.DTOs.Food;
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
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepository _foodRepo;
        private readonly IMapper _mapper;
        public FoodController(IFoodRepository foodRepo, IMapper mapper)
        {
            _foodRepo = foodRepo;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {

            var foodModels = await _foodRepo.GetAllAsync();

            if (foodModels == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<FoodDto>>(foodModels));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var foodModel = await _foodRepo.GetAsync(id);

            if (foodModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FoodDto>(foodModel));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateFoodRequestDto foodDto)
        {
            if (foodDto == null) return BadRequest("Food object is null");

            var foodModel = await _foodRepo.AddAsync(_mapper.Map<Food>(foodDto));
            var food = _mapper.Map<FoodDto>(foodModel);
            return CreatedAtAction(nameof(Get), new { id = foodModel.Id }, food);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Put(int id, FoodDto foodDto)
        {
            if (id != foodDto.Id) return BadRequest("Food Ids do not match");

            var foodModel = await _foodRepo.GetAsync(id);
            if (foodModel == null) return BadRequest("Food not found");

            _mapper.Map(foodDto, foodModel);

            try
            {
                await _foodRepo.UpdateAsync(foodModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _foodRepo.Exists(id))
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
            if (!await _foodRepo.Exists(id))
            {
                return NotFound("Food not found.");
            }

            await _foodRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
