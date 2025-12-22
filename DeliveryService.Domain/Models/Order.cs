using System;
using System.Collections.Generic;
using DeliveryService.Domain.Enum; // Важно для StatusOrder

namespace DeliveryService.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; } // Было пропущено
        public User User { get; set; }

        public DateTime DateCreated { get; set; } // Было пропущено

        public StatusOrder Status { get; set; } // Было пропущено

        // Оставляем только ОДНО определение Items
        public virtual List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}