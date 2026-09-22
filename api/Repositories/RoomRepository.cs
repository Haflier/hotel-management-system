using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using apiRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public RoomRepository(ApplicationDbContext context, IMapper mapper)
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
                .Include(a => a.ActiveServices)
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (roomModel == null)
            {
                return null;
            }

            return roomModel;
        }

        public async Task<Room> Delete(int? id)
        {
            var roomModel = await _context.Rooms
                .Include(a => a.ActiveServices)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (roomModel == null)
            {
                return null;
            }

            _context.Services.RemoveRange(roomModel.ActiveServices);
            _context.Rooms.Remove(roomModel);
            await _context.SaveChangesAsync();

            return roomModel;
        }

        public async Task<RoomService> AddServiceToRoomAsync(int roomId, int serviceId)
        {
            var roomModel = await _context.Rooms
                .Include(a => a.ActiveServices)
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            var service = await _context.Services.FindAsync(serviceId);

            if (roomModel != null && service != null && !roomModel.ActiveServices.Contains(service))
            {
                roomModel.ActiveServices.Add(service);
                await _context.SaveChangesAsync();
                return new RoomService { RoomId = roomModel.Id, ServiceId = service.Id };
            }
            else
            {
                return null;
            }
        }

        public async Task<RoomService> RemoveServiceFromRoomAsync(int roomId, int serviceId)
        {
            var roomModel = await _context.Rooms
                .Include(a => a.ActiveServices)
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            var service = roomModel?.ActiveServices.FirstOrDefault(s => s.Id == serviceId);

            if (service != null)
            {
                roomModel?.ActiveServices.Remove(service);
                await _context.SaveChangesAsync();
                return new RoomService { RoomId = roomModel.Id, ServiceId = service.Id };
            }
            else
            {
                return null;
            }
        }
    }
}
