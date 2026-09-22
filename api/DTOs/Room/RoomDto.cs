using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOsa.Reservation;

namespace api.DTOs.Room
{
    public class RoomDto : RoomBaseDto
    {
        public int Id { get; set; }
        public int HotelId { get; set; }

        public ICollection<ReservationForRoomDto> Reservations { get; set; } = new List<ReservationForRoomDto>();
    }
}
