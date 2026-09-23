using System.ComponentModel.DataAnnotations.Schema;


namespace api.Models
{
    [Table("Drink")]
    public class Drink
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string Meal { get; set; } = string.Empty;
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;
    }
}
