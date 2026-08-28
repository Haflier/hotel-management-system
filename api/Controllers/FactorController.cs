using api.DTOs.Factor;
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
    public class FactorController : ControllerBase
    {
        private readonly IFactorRepository _factorRepo;
        private readonly IMapper _mapper;
        public FactorController(IFactorRepository factorRepo, IMapper mapper)
        {
            _factorRepo = factorRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAll()
        {
            var factors = await _factorRepo.GetAllAsync();

            if (factors == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<FactorDto>>(factors));
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> Get(int id)
        {
            var factor = await _factorRepo.GetAsync(id);

            if (factor == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FactorDto>(factor));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateFactorRequestDto factorDto)
        {
            var factorModel = await _factorRepo.AddAsync(_mapper.Map<Factor>(factorDto));
            var factor = _mapper.Map<FactorDto>(factorModel);
            return CreatedAtAction(nameof(Get), new { id = factorModel.Id }, factor);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Put(int id, UpdateFactorRequestDto factorDto)
        {
            if (id != factorDto.Id) return BadRequest("Factor Ids do not match");

            var factorModel = await _factorRepo.GetAsync(id);
            if (factorModel == null) return BadRequest("Factor not found");

            _mapper.Map(factorDto, factorModel);

            try
            {
                await _factorRepo.UpdateAsync(factorModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _factorRepo.Exists(id))
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
            if (!await _factorRepo.Exists(id))
            {
                return NotFound("Factor not found.");
            }

            await _factorRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
