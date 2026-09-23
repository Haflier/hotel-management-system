using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Food;

namespace api.DTOs.Drink
{
    public class DrinkBaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Meal { get; set; } = string.Empty;
        public int HotelId { get; set; }
    }
}
