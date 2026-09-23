namespace api.DTOs.OrderItem
{
    public class CreateOrderItemRequestDto
    {
        public int Quantity { get; set; }
        public int? FoodId { get; set; }
        public int? DrinkId { get; set; }
        public int ReservationId { get; set; }
    }
}
