using System;
using DeliveryService.Domain.Models; // Для User
using DeliveryService.Domain.Enum;   // ОБЯЗАТЕЛЬНО: Тут лежит StatusOrder

namespace DeliveryService.Domain.Models
{
    public class Request
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public string PathImg { get; set; }

        // Исправлено: Status -> StatusOrder
        public StatusOrder Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
    }
}