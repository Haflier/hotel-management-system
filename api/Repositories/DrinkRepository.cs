using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using apiRepositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class DrinkRepository : GenericRepository<Drink>, IDrinkRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DrinkRepository(ApplicationDbContext context, IMapper mapper)
        : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<Drink>> GetByHotelIdAsync(int hotelId)
        {
            return await _context.Drinks
                .Where(d => d.HotelId == hotelId)
                .ToListAsync();
        }
    }
}
