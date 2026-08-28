using api.DTOs.Drink;
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
    public class DrinkController : ControllerBase
    {
        private readonly IDrinkRepository _drinkRepo;
        private readonly IMapper _mapper;
        public DrinkController(IDrinkRepository drinkRepo, IMapper mapper)
        {
            _drinkRepo = drinkRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var drinks = await _drinkRepo.GetAllAsync();

            if (drinks == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<DrinkDto>>(drinks));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var drink = await _drinkRepo.GetAsync(id);

            if (drink == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DrinkDto>(drink));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateDrinkRequestDto drinkDto)
        {
            if (drinkDto == null) return BadRequest("Drink object is null");

            var drinkModel = await _drinkRepo.AddAsync(_mapper.Map<Drink>(drinkDto));
            var drink = _mapper.Map<DrinkDto>(drinkModel);
            return CreatedAtAction(nameof(Get), new { id = drinkModel.Id }, drink);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Put(int id, DrinkDto drinkDto)
        {
            if (id != drinkDto.Id) return BadRequest("Drink Ids do not match");

            var drinkModel = await _drinkRepo.GetAsync(id);
            if (drinkModel == null) return BadRequest("Drink not found");

            _mapper.Map(drinkDto, drinkModel);

            try
            {
                await _drinkRepo.UpdateAsync(drinkModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _drinkRepo.Exists(id))
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
            if (!await _drinkRepo.Exists(id))
            {
                return NotFound("Drink not found.");
            }

            await _drinkRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
