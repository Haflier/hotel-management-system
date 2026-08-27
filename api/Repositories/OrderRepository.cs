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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public OrderRepository(ApplicationDbContext context, IMapper mapper)
        : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Order> CheckExistingOrder(string userId)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.ApiUserId == userId && !o.IsFinalized);

            if (existingOrder == null)
            {
                return null;
            }

            return existingOrder;
        }

        public async Task<ICollection<Order>> GetAllDetails()
        {
            var orderModels = await _context.Orders
            .Include(o => o.Items)
            .ToListAsync();

            if (orderModels == null)
            {
                return null;
            }

            return orderModels;
        }    

        public async Task<Order> GetDetail(int orderId)
        {
            var orderModel = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (orderModel == null)
            {
                return null;
            }

            return orderModel;
        }
    }
}
