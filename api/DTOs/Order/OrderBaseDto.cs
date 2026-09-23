using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.OrderItem;

namespace api.DTOs.Order
{
    public class OrderBaseDto
    {
        public DateTime CreatedAt { get; set; }
        public int ReservationId { get; set; }
        public ICollection<OrderItemForOrderDto> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsFinalized { get; set; }
    }
}
