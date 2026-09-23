using api.Models;

namespace api.Interfaces
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        int ConvertToDeterministicRandomNumber(string input);

        Task<OrderItem?> GetUserOrderItemAsync(
            int orderItemId,
            string userId);
    }
}
