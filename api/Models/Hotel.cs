using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Hotel")]
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int CityId { get; set; }
        public City? City { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Food> Foods { get; set; } = new List<Food>();
        public ICollection<Drink> Drinks { get; set; } = new List<Drink>();
    }
}
