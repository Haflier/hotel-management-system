using System.Security.Cryptography;
using System.Text;
using api.Data;
using api.Interfaces;
using api.Models;
using apiRepositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderItemRepository(
            ApplicationDbContext context,
            IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int ConvertToDeterministicRandomNumber(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(
                    Encoding.UTF8.GetBytes(input));

                int value = BitConverter.ToInt32(hash, 0);

                return Math.Abs(value);
            }
        }

        public async Task<OrderItem?> GetUserOrderItemAsync(
            int orderItemId,
            string userId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi =>
                    oi.Id == orderItemId &&
                    oi.Order.ApiUserId == userId);
        }
    }
}
