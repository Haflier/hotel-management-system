using api.Data;
using api.Interfaces;
using api.Models;
using apiRepositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class FactorRepository : GenericRepository<Factor>, IFactorRepository
    {
        private readonly ApplicationDbContext _context;

        public FactorRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
        }

        public async Task<bool> ExistsByApiUserIdAsync(string apiUserId)
        {
            return await _context.Factors
                .AnyAsync(f => f.ApiUserId == apiUserId);
        }
    }
}
