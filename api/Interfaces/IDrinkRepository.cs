using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IDrinkRepository : IGenericRepository<Drink>
    {
        Task<List<Drink>> GetByHotelIdAsync(int hotelId);
    }
}
