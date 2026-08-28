using api.DTOs.Service;
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
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly IMapper _mapper;
        public ServiceController(IServiceRepository serviceRepo, IMapper mapper)
        {
            _serviceRepo = serviceRepo;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var serviceModels = await _serviceRepo.GetAllAsync();

            if (serviceModels == null)
            {
                return NotFound();
            }

            var servicesDto = _mapper.Map<List<ServiceDto>>(serviceModels);
            return Ok(servicesDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var serviceModel = await _serviceRepo.GetAsync(id);

            if (serviceModel == null)
            {
                return NotFound();
            }

            var serviceDto = _mapper.Map<ServiceDto>(serviceModel);
            return Ok(serviceDto);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateServiceRequestDto serviceDto)
        {
            var serviceModel = await _serviceRepo.AddAsync(_mapper.Map<Service>(serviceDto));
            var service = _mapper.Map<ServiceDto>(serviceModel);
            return CreatedAtAction(nameof(Get), new { id = serviceModel.Id }, service);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Put(int id, ServiceDto serviceDto)
        {
            if (id != serviceDto.Id) return BadRequest("Service Ids do not match");

            var serviceModel = await _serviceRepo.GetAsync(id);
            if (serviceModel == null) return BadRequest("Service not found");

            _mapper.Map(serviceDto, serviceModel);

            try
            {
                await _serviceRepo.UpdateAsync(serviceModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _serviceRepo.Exists(id))
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
            if (!await _serviceRepo.Exists(id))
            {
                return NotFound("Service not found.");
            }

            await _serviceRepo.DeleteAsync(id);
            return NoContent();
        }

    }
}
