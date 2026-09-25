using api.Models;

namespace api.Interfaces
{
    public interface IFactorRepository : IGenericRepository<Factor>
    {
        Task<bool> ExistsByApiUserIdAsync(string apiUserId);
    }
}
