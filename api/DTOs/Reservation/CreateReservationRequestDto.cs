namespace api.DTOs.Reservation
{
    public class CreateReservationRequestDto
    {
        public DateTimeOffset CheckinDate { get; set; }
        public DateTimeOffset CheckOutDate { get; set; }
        public int RoomId { get; set; }
    }
}
