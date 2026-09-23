namespace api.DTOs.Reservation
{
    public class ReservationBaseDto
    {
        public DateTime CheckinDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal PricePerDay { get; set; }
        public int TotalNights { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
