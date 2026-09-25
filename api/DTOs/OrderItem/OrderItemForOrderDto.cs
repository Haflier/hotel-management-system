namespace api.DTOs.OrderItem
{
    public class OrderItemForOrderDto : OrderItemBaseDto
    {
        public int Id { get; set; }
        public int? FoodId { get; set; }
        public string? FoodName { get; set; }
        public int? DrinkId { get; set; }
        public string? DrinkName { get; set; }
    }
}
