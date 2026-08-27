using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using apiRepositories;
using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ReservationRepository(ApplicationDbContext context, IMapper mapper)
        : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<List<DateTime>> DateTimeCalculation(DateTime checkinDate, DateTime checkOutDate)
        {
            var totalNights = (checkOutDate - checkinDate).Days;
            var bookedDates = new List<DateTime>();

            // include the check-in day
            bookedDates.Add(checkinDate);

            // add each subsequent day up until check-out (exclusive)
            for (var day = 1; day < totalNights; day++)
            {
                bookedDates.Add(checkinDate.AddDays(day));
            }

            return Task.FromResult(bookedDates);
        }

        public async Task<Room?> GetRoomAsync(int id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Reservation>> GetReservationsByRoomId(int roomId, DateTime checkinDate, DateTime checkOutDate)
        {
            // Only fetch reservations that could possibly overlap
            return await _context.Reservations.Where(r => r.RoomId == roomId &&
                    r.CheckinDate < checkOutDate &&  // reservation ends after the requested start
                    r.CheckOutDate > checkinDate)    // reservation starts before the requested end
                        .ToListAsync();
        }

    }
}
