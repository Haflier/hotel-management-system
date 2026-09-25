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
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoomRepository(
            ApplicationDbContext context,
            IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Rooms
                .Include(r => r.Reservations)
                .ToListAsync();
        }

        public async Task<Room> GetDetails(int? id)
        {
            var roomModel = await _context.Rooms
                .Include(r => r.Reservations)
                .Include(r => r.RoomServices)
                    .ThenInclude(rs => rs.Service)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (roomModel == null)
            {
                return null;
            }

            roomModel.ActiveServices = roomModel.RoomServices
                .Where(rs => rs.Service != null)
                .Select(rs => rs.Service)
                .ToList();

            return roomModel;
        }

        public async Task<Room> Delete(int? id)
        {
            var roomModel = await _context.Rooms
                .Include(r => r.RoomServices)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (roomModel == null)
            {
                return null;
            }

            _context.RoomServices.RemoveRange(roomModel.RoomServices);
            _context.Rooms.Remove(roomModel);

            await _context.SaveChangesAsync();

            return roomModel;
        }

        public async Task<RoomService> AddServiceToRoomAsync(
            int roomId,
            int serviceId)
        {
            var roomExists = await _context.Rooms
                .AnyAsync(r => r.Id == roomId);

            var serviceExists = await _context.Services
                .AnyAsync(s => s.Id == serviceId);

            if (!roomExists || !serviceExists)
            {
                return null;
            }

            var alreadyAssigned = await _context.RoomServices
                .AnyAsync(rs =>
                    rs.RoomId == roomId &&
                    rs.ServiceId == serviceId);

            if (alreadyAssigned)
            {
                return null;
            }

            var roomService = new RoomService
            {
                RoomId = roomId,
                ServiceId = serviceId,
            };

            _context.RoomServices.Add(roomService);
            await _context.SaveChangesAsync();

            return roomService;
        }

        public async Task<RoomService> RemoveServiceFromRoomAsync(
            int roomId,
            int serviceId)
        {
            var roomService = await _context.RoomServices
                .FirstOrDefaultAsync(rs =>
                    rs.RoomId == roomId &&
                    rs.ServiceId == serviceId);

            if (roomService == null)
            {
                return null;
            }

            _context.RoomServices.Remove(roomService);
            await _context.SaveChangesAsync();

            return roomService;
        }
    }
}
