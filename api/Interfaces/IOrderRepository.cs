using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> CheckExistingOrder(string userId);
        Task<ICollection<Order>> GetAllDetails();
        Task<Order> GetDetail(int orderId);
        Task<Order?> GetUserOrderDetail(int orderId, string userId);
    }
}
