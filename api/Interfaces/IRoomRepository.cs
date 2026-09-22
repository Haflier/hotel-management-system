using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces 
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<List<Room>> GetAllAsync();
        Task<Room> GetDetails(int? id);
        Task<Room> Delete(int? id);
        Task<RoomService> AddServiceToRoomAsync(int roomId, int serviceId);
        Task<RoomService> RemoveServiceFromRoomAsync(int roomId, int serviceId);
    }
}
